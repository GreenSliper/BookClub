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
		private readonly MemberPermissions minimalToManageMembers = MemberPermissions.Manager;
		public MemberPermissions MinimalToManage => minimalToManage;
		public MemberPermissions DefaultPermission => MemberPermissions.Reader;

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

		public bool CanUserManageClub(Club club, string userId) => HasUserPermission(club, userId, minimalToManage);
		public bool CanUserManageClubMembers(Club club, string userId) => HasUserPermission(club, userId, minimalToManageMembers);

		public bool CanUserViewClub(Club club, string userId)
		{
			if (club == null)
				return false;
			if (club.IsPublic || club.Members.Any(x => x.UserID == userId))
				return true;
			return false;
		}

		public bool CanUserGivePermission(ClubMember sender, MemberPermissions givenPermission)
		{
			return sender.PermissionLevel > givenPermission || sender.Club.Creator == sender.User;
		}

		public bool CanUserJoinPublicClub(Club club, string userId)
		{
			if (club.IsPublic && !club.Members.Any(x => x.UserID == userId))
				return true;
			return false;
		}

		public bool CanUserManageClub(ClubMember member)
		{
			return member.PermissionLevel >= minimalToManage || member.UserID == member.Club.Creator.Id;
		}

		public bool CanUserModifyMember(ClubMember manager, ClubMember member)
		{
			return CanUserManageClubMembers(manager.Club, manager.UserID) &&
				   CanUserGivePermission(manager, member.PermissionLevel) &&
				   member.UserID != manager.Club.Creator.Id;
		}
	}
}