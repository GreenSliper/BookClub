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
		private readonly IRepository<DAL.DTO.Club, int> clubRepos;

		private readonly MemberPermissions minimalToManage = MemberPermissions.Manager;
		public MemberPermissions MinimalToManage => minimalToManage;
		public MemberPermissions DefaultPermission => MemberPermissions.Reader;

		public AccessService(IRepository<DAL.DTO.Club, int> clubRepos)
		{
			this.clubRepos = clubRepos;
		}

		public bool CanUserManageClub(Club club, string userId)
		{
			if (club == null)
				return false;
			if (club.Creator?.Id == userId)
				return true;
			DAL.DTO.MemberPermissions? perm;
			if ((perm = club.Members.FirstOrDefault(x => x.UserID == userId)?.PermissionLevel) != null)
			{
				if ((int)perm >= (int)minimalToManage)
					return true;
			}
			return false;
		}

		public async Task<ModelActionRequestResult<Club>> CanUserManageClub(int clubId, string userId)
		{
			var club = await clubRepos.Get(clubId);
			if (CanUserManageClub(club, userId))
				return new ModelActionRequestResult<Club>(true, club);
			return new ModelActionRequestResult<Club>(false);
		}

		public async Task<ModelActionRequestResult<Club>> CanUserViewClub(int clubId, string userId)
		{
			var club = await clubRepos.Get(clubId);
			if (CanUserViewClub(club, userId))
				return new ModelActionRequestResult<Club>(true, club);
			return new ModelActionRequestResult<Club>(false);
		}

		public bool CanUserViewClub(Club club, string userId)
		{
			if (club == null)
				return false;
			if (club.IsPublic || club.Members.Any(x => x.UserID == userId))
				return true;
			return false;
		}

		public async Task<ModelActionRequestResult<Club>> CanUserJoinClub(int clubId, string userId)
		{
			var club = await clubRepos.Get(clubId);
			if (club.IsPublic && !club.Members.Any(x => x.UserID == userId))
				return new ModelActionRequestResult<Club>(true, club);
			return new ModelActionRequestResult<Club>(false);
		}
	}
}
