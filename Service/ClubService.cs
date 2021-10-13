using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Data;
using DAL.Models;

namespace Service
{
	public class ClubService : IClubService
	{
		private readonly IRepository<ReaderUser> userRepos;
		private readonly IMapper mapper;
		public ClubService(IRepository<ReaderUser> userRepos, IMapper mapper)
		{
			this.userRepos = userRepos;
			this.mapper = mapper;
		}
		public async Task<IEnumerable<Club>> GetUserClubs(string userId)
		{
			if (int.TryParse(userId, out int id))
			{
				//select clubs from membership and map from DTO to model
				return from m in (await userRepos.Get(id)).Memberships
					   select mapper.Map<Club>(m.Club);
			}
			return null;
		}
	}
}
