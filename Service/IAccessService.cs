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
		Task<ModelAccessResult<Club, Ban, AccessErrors>> GetClub(Club club, string userId, MemberActions targetAction);
		Task<ModelAccessResult<Club, Ban, AccessErrors>> CanUserJoinPublicClub(Club club, string userId);
		bool CanUserManageClub(ClubMember member);
		bool CanUserGivePermission(ClubMember sender, MemberPermissions givenPermission);
		Task<bool> CanUserModifyMember(ClubMember manager, ClubMember member);
		Task AddOrUpdateBan(Ban ban);
		MemberPermissions DefaultPermission { get; }
	}
	public enum MemberActions { View, ManageClub, ManageMembers, Edit }
}
