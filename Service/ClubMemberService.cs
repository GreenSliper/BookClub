using AutoMapper;
using DAL.Data;
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
		private readonly IMapper mapper;
		private readonly IAccessService accessService;
		public ClubMemberService(IRepository<ReaderUser, string> userRepos,
			IRepository<DAL.DTO.Club, int> clubRepos, IAccessService accessService, IMapper mapper)
		{
			this.userRepos = userRepos;
			this.clubRepos = clubRepos;
			this.accessService = accessService;
			this.mapper = mapper;
		}
		public async Task<bool> JoinClub(int clubId, string userId)
		{
			var request = await accessService.CanUserJoinClub(clubId, userId);
			if (request.successful)
			{
				request.requestedModel.Members.Add(new DAL.DTO.ClubMember()
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
	}
}
