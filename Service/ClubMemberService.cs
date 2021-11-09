using AutoMapper;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class ClubMemberService : IClubMemberService
	{
		private readonly IRepository<ReaderUser, string> userRepos;
		private readonly IRepository<DAL.DTO.Club, int> clubRepos;
		IExpirableRepos<DAL.DTO.ClubInvite, (int clubId, string receiverId)> inviteRepos;
		IRepository<DAL.DTO.ClubMember, (int clubId, string userId)> memberRepos;
		private readonly IMapper mapper;
		private readonly IAccessService accessService;
		public ClubMemberService(IRepository<ReaderUser, string> userRepos,
			IRepository<DAL.DTO.Club, int> clubRepos,
			IExpirableRepos<DAL.DTO.ClubInvite, (int clubId, string receiverId)> inviteRepos,
			IRepository<DAL.DTO.ClubMember, (int clubId, string userId)> memberRepos,
			IAccessService accessService, IMapper mapper)
		{
			this.userRepos = userRepos;
			this.clubRepos = clubRepos;
			this.inviteRepos = inviteRepos;
			this.memberRepos = memberRepos;
			this.accessService = accessService;
			this.mapper = mapper;
		}

		public async Task<bool> JoinClub(int clubId, string userId)
		{
			var club = await clubRepos.Get(clubId);
			if ((await accessService.CanUserJoinPublicClub(club, userId)).Success)
			{
				club.Members.Add(new DAL.DTO.ClubMember()
				{
					ClubID = clubId,
					UserID = userId,
					PermissionLevel = accessService.DefaultPermission
				});
				await clubRepos.SaveChanges();
				return true;
			}
			return false;
		}

		public async Task<bool> LeaveClub(int clubId, string userId)
		{
			var club = await clubRepos.Get(clubId);
			var member = club.Members.FirstOrDefault(x => x.UserID == userId);
			if (member != null)
			{
				club.Members.Remove(member);
				await clubRepos.SaveChanges();
				return true;
			}
			return false;
		}
		public async Task<IEnumerable<ClubInvite>> GetUserReceivedInvites(string userId)
		{
			var user = await userRepos.Get(userId);
			var result = await inviteRepos.GetNotExpired(user.ReceivedInvites);
			return result.OrderByDescending(x=>x.ExpirationTime).Select(x=>mapper.Map<ClubInvite>(x));
		}

		async Task<bool> ProcessInvite(int clubId, string receiverId, bool accept)
		{
			var user = await userRepos.Get(receiverId);
			if (user == null)
				return false;
			var invite = await inviteRepos.GetNotExpired(user.ReceivedInvites
				.FirstOrDefault(x => x.ClubID == clubId));
			if (invite != null)
			{
				if (accept)
				{
					user.Memberships.Add(new DAL.DTO.ClubMember()
					{
						ClubID = clubId,
						PermissionLevel = invite.GivenPermissions,
						UserID = receiverId
					});
					await userRepos.SaveChanges();
				}
				await inviteRepos.Delete(invite);
				return true;
			}
			return false;
		}

		public async Task<bool> TryAcceptInvite(int clubId, string userId)
		{
			return await ProcessInvite(clubId, userId, accept: true);
		}

		public async Task<bool> TryDeclineInvite(int clubId, string userId)
		{
			return await ProcessInvite(clubId, userId, accept: false);
		}

		async Task<bool> CanUserReceiveInvite(ReaderUser receiver, ClubInvite invite, ModelStateDictionary modelState)
		{
			if (receiver == null)
			{
				modelState.AddModelError("", "User with given name not found!");
				return false;
			}
			if (receiver.Memberships.Any(x => x.ClubID == invite.ClubID))
			{
				modelState.AddModelError("", "User is already a member of this club!");
				return false;
			}
			if ((await inviteRepos.GetNotExpired(receiver.ReceivedInvites))
				.Any(x => x.ClubID == invite.ClubID))
			{
				modelState.AddModelError("", "User is already invited to this club!");
				return false;
			}
			return true;
		}

		public async Task<bool> TrySendInvite(ClubInvite invite, ModelStateDictionary modelState, string inviterID)
		{
			if (!modelState.IsValid)
				return false;
			var club = await clubRepos.Get(invite.ClubID);
			var receiver = (await userRepos.GetAll()).FirstOrDefault
				(x => x.NormalizedUserName == invite.ReceiverName.ToUpper());

			if (!await CanUserReceiveInvite(receiver, invite, modelState))
				return false;
				
			var sender = club.Members.FirstOrDefault(x => x.UserID == inviterID);
			if (sender != null && 
				accessService.CanUserGivePermission(sender, invite.GivenPermissions))
			{
				var dto = mapper.Map<DAL.DTO.ClubInvite>(invite);
				dto.ReceiverID = receiver.Id;
				dto.InviterID = inviterID;
				await inviteRepos.Insert(dto);
				return true;
			}
			else
				modelState.AddModelError("", "You are not able to give this permission level!");
			return false;
		}

		public async Task<ModelAccessResult<ClubMember, Ban, AccessErrors>> ManageMember(string managerId, string targetUserId, int clubId)
		{
			var club = await clubRepos.Get(clubId);
			var manageMemberRequest = await accessService.CanUserManageClubMembers(club, managerId);
			if (!manageMemberRequest.Success)
				return manageMemberRequest.Map<ClubMember, Ban>(mapper);
			if (manageMemberRequest.Success)
			{
				var target = club.Members.FirstOrDefault(x => x.UserID == targetUserId);
				if (target != null)
					return new ModelAccessResult<ClubMember, Ban, AccessErrors>(mapper.Map<ClubMember>(target));
				else
					return new ModelAccessResult<ClubMember, Ban, AccessErrors>(AccessErrors.NotFound);
			}
			return new ModelAccessResult<ClubMember, Ban, AccessErrors>(AccessErrors.NotPermitted);
		}

		public async Task<bool> TryUpdateMember(ClubMember member, ModelStateDictionary modelState, string managerId)
		{
			if (!modelState.IsValid)
				return false;
			var club = await clubRepos.Get(member.Club.ID.Value);
			if (club == null)
				return false;
			var manager = club.Members.FirstOrDefault(x => x.UserID == managerId);
			if (manager == null)
				return false;
			var oldMember = club.Members.FirstOrDefault(x => x.UserID == member.User.Id);
			if ((await accessService.CanUserModifyMember(manager, oldMember)))
			{
				if (accessService.CanUserGivePermission(manager, member.PermissionLevel))
				{
					var newMember = mapper.Map<DAL.DTO.ClubMember>(member);
					await memberRepos.Update(oldMember, newMember);
					return true;
				}
				else
					modelState.AddModelError("", "You cannot give this permission level!");
			}
			else
				modelState.AddModelError("", "You cannot modify this user!");
			return false;
		}

		public async Task<bool> Ban(Ban ban, ModelStateDictionary modelState, string managerId)
		{
			if (!modelState.IsValid)
				return false;
			var club = await clubRepos.Get(ban.ClubID);
			if ((await accessService.CanUserViewClub(club, ban.BannedUserID)).error == AccessErrors.Banned)
			{
				modelState.AddModelError("", "User is already banned!");
				return false;
			}
			var dto = mapper.Map<DAL.DTO.Ban>(ban);
			if (ban.Forever)
				dto.ExpirationTime = DateTime.MaxValue;
			dto.BannedByID = managerId;
			await accessService.AddOrUpdateBan(dto);
			return true;
		}
	}
}
