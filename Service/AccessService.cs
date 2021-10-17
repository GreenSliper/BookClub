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
		private readonly IRepository<ReaderUser, string> userRepos;
		private readonly IRepository<DAL.DTO.Club, int> clubRepos;

		private readonly MemberPermissions minimalToManage = MemberPermissions.Manager;
		public MemberPermissions MinimalToManage => minimalToManage;

		public AccessService(IRepository<ReaderUser, string> userRepos, IRepository<DAL.DTO.Club, int> clubRepos)
		{
			this.userRepos = userRepos;
			this.clubRepos = clubRepos;
		}

		public async Task<ModelActionRequestResult<Club>> CanUserManageClub(int clubId, string userId)
		{
			var club = await clubRepos.Get(clubId);
			if (club == null)
				return new ModelActionRequestResult<Club>(false);
			if (club.Creator?.Id == userId)
				return new ModelActionRequestResult<Club>(true, club);
			DAL.DTO.MemberPermissions? perm;
			if ((perm = club.Members.FirstOrDefault(x => x.UserID == userId)?.PermissionLevel) != null)
			{
				if ((int)perm >= (int)minimalToManage)
					return new ModelActionRequestResult<Club>(true, club);
			}
			return new ModelActionRequestResult<Club>(false);
		}

		public async Task<ModelActionRequestResult<Club>> CanUserViewClub(int clubId, string userId)
		{
			var club = await clubRepos.Get(clubId);
			if (club == null)
				return new ModelActionRequestResult<Club>(false);
			if (club.IsPublic || club.Members.Any(x => x.UserID == userId))
				return new ModelActionRequestResult<Club>(true, club);
			return new ModelActionRequestResult<Club>(false);
		}
	}
}
