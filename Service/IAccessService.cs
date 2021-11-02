using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public interface IAccessService
	{
		bool CanUserManageClubMembers(Club club, string userId);
		bool CanUserJoinPublicClub(Club club, string userId);
		bool CanUserManageClub(Club club, string userId);
		bool CanUserManageClub(ClubMember member);
		bool CanUserModifyMember(ClubMember manager, ClubMember member);
		bool CanUserViewClub(Club club, string userId);
		bool CanUserGivePermission(ClubMember sender, MemberPermissions givenPermission);
		MemberPermissions MinimalToManage { get; }
		MemberPermissions DefaultPermission { get; }
	}
}
