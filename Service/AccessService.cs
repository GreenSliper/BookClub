using DAL.Data;
using DAL.DTO;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class AccessService : IAccessService
	{
		private readonly MemberPermissions minimalToManage = MemberPermissions.Manager;
		public MemberPermissions DefaultPermission => MemberPermissions.Reader;

		private readonly IExpirableRepos<Ban, (int clubId, string bannedUserID)> banRepos;

		bool[,] actionPermissionMatrix;
		public AccessService(IExpirableRepos<Ban, (int clubId, string bannedUserID)> banRepos)
		{
			this.banRepos = banRepos;
			//Permissions: Reader, Manager, Admin, Creator
			actionPermissionMatrix = new bool[,] {
				/*View*/		 { true, true, true, true },
				/*Manage*/		 { false, true, true, true },
				/*ManageMembers*/{ false, false, true, true },
				/*Edit*/		 { false, false, false, true }
			};
		}

		private bool HasUserPermission(Club club, string userId, MemberPermissions permission)
		{
			if (club == null)
				return false;
			if (club.Creator?.Id == userId)
				return true;
			DAL.DTO.MemberPermissions? perm;
			if ((perm = club.Members.FirstOrDefault(x => x.UserID == userId)?.PermissionLevel) != null)
			{
				if (perm >= permission)
					return true;
			}
			return false;
		}

		private async Task<Ban> GetUserBan(Club club, string userId)
		{
			return (await banRepos.GetNotExpired(club.BanList)).FirstOrDefault(x => x.BannedUserID == userId);
		}
		private async Task<ModelAccessResult<Club, Ban, AccessErrors>> GetClubOrBan(Club club, string userId)
		{
			var ban = await GetUserBan(club, userId);
			if (ban != null)
				return new ModelAccessResult<Club, Ban, AccessErrors>(ban, AccessErrors.Banned);
			return new ModelAccessResult<Club, Ban, AccessErrors>(club);
		}

		public bool CanUserGivePermission(ClubMember sender, MemberPermissions givenPermission)
		{
			return sender.PermissionLevel > givenPermission || sender.Club.Creator == sender.User;
		}

		public async Task<ModelAccessResult<Club, Ban, AccessErrors>> CanUserJoinPublicClub(Club club, string userId)
		{
			if (club.IsPublic && !club.Members.Any(x => x.UserID == userId))
				return await GetClubOrBan(club, userId);
			return new ModelAccessResult<Club, Ban, AccessErrors>(AccessErrors.NoAccess);
		}

		public bool CanUserManageClub(ClubMember member)
		{
			return member.PermissionLevel >= minimalToManage || member.UserID == member.Club.Creator.Id;
		}

		public async Task<bool> CanUserModifyMember(ClubMember manager, ClubMember member)
		{
			return (await GetClub(manager.Club, manager.UserID, MemberActions.ManageMembers)).Success &&
				   CanUserGivePermission(manager, member.PermissionLevel) &&
				   member.UserID != manager.Club.Creator.Id;
		}

		public async Task AddOrUpdateBan(Ban ban)
		{
			var oldBan = await banRepos.Get((ban.ClubID, ban.BannedUserID));
			if (oldBan == null)
				await banRepos.Insert(ban);
			else
				await banRepos.Update(oldBan, ban);
		}

		public async Task<ModelAccessResult<Club, Ban, AccessErrors>> GetClub(Club club, string userId, MemberActions targetAction)
		{
			if (club == null)
				return new ModelAccessResult<Club, Ban, AccessErrors>(AccessErrors.NotFound);
			if (club.IsPublic || club.Members.Any(x => x.UserID == userId))
			{
				var ban = await GetUserBan(club, userId);
				if (ban != null)
					return new ModelAccessResult<Club, Ban, AccessErrors>(ban, AccessErrors.Banned);
				
				var member = club.Members.FirstOrDefault(x => x.UserID == userId);
				if(member == null)
					return new ModelAccessResult<Club, Ban, AccessErrors>(AccessErrors.NotFound);

				if (actionPermissionMatrix[(int)targetAction, (int)member.PermissionLevel])
					return new ModelAccessResult<Club, Ban, AccessErrors>(club);
				else
					return new ModelAccessResult<Club, Ban, AccessErrors>(AccessErrors.NotPermitted);
			}
			return new ModelAccessResult<Club, Ban, AccessErrors>(AccessErrors.NoAccess);
		}
	}
}