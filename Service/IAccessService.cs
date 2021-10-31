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
		Task<ModelActionRequestResult<Club>> CanUserJoinClub(int clubId, string userId);
		Task<ModelActionRequestResult<Club>> CanUserViewClub(int clubId, string userId);
		Task<ModelActionRequestResult<Club>> CanUserManageClub(int clubId, string userId);
		bool CanUserManageClub(Club club, string userId);
		bool CanUserViewClub(Club club, string userId);
		bool CanUserGivePermission(ClubMember sender, MemberPermissions givenPermission);
		MemberPermissions MinimalToManage { get; }
		MemberPermissions DefaultPermission { get; }
	}
}
