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
	public class DiscussionService : IDiscussionService
	{
		private readonly IRepository<ReaderUser, string> userRepos;
		private readonly IRepository<DAL.DTO.Club, int> clubRepos;
		private readonly IMapper mapper;
		public DiscussionService(IRepository<ReaderUser, string> userRepos, IRepository<DAL.DTO.Club, int> clubRepos, IMapper mapper)
		{
			this.userRepos = userRepos;
			this.clubRepos = clubRepos;
			this.mapper = mapper;
		}
	}
}
