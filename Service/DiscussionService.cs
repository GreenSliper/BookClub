using AutoMapper;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
		private readonly IRepository<DAL.DTO.ClubDiscussion, int> discussionRepos;
		private readonly IAccessService accessService;
		private readonly IMapper mapper;
		public DiscussionService(IRepository<ReaderUser, string> userRepos, IRepository<DAL.DTO.Club, int> clubRepos,
			IRepository<DAL.DTO.ClubDiscussion, int> discussionRepos, IAccessService accessService, IMapper mapper)
		{
			this.userRepos = userRepos;
			this.clubRepos = clubRepos;
			this.discussionRepos = discussionRepos;
			this.accessService = accessService;
			this.mapper = mapper;
		}

		public async Task<bool> TryAddBooks(IEnumerable<int> bookIDs, int discId, string userId)
		{
			var disc = await discussionRepos.Get(discId);
			var request = await accessService.CanUserManageClub(disc.Club.ID, userId);
			if (!request.successful)
				return false;
			foreach (var bookId in bookIDs)
				disc.Books.Add(new DAL.DTO.ClubDiscussionBook()
				{
					BookID = bookId,
					DiscussionID = discId
				});
			await discussionRepos.Update(disc);
			return true;
		}

		public async Task<ModelActionRequestResult<ClubDiscussion>> TryAddDiscussion
			(ClubDiscussion discussion, ModelStateDictionary modelState, int clubId, string userId)
		{
			//Maybe replace with discussionRepos? TODO
			if (!modelState.IsValid)
				return new ModelActionRequestResult<ClubDiscussion>(false);
			DAL.DTO.Club club = null;
			club = await clubRepos.Get(clubId);
			if (club == null)
				return new ModelActionRequestResult<ClubDiscussion>(false);
			ReaderUser user = await userRepos.Get(userId);
			discussion.Creator = user;
			var dto = mapper.Map<DAL.DTO.ClubDiscussion>(discussion);
			club.Discussions.Add(dto);
			await clubRepos.Update(club);
			//ID will be auto-updated
			return new ModelActionRequestResult<ClubDiscussion>(true, mapper.Map<ClubDiscussion>(dto));
		}

		public async Task<ClubDiscussion> TryGetDiscussion(int discId, string userId)
		{
			var dto = await discussionRepos.Get(discId);
			if (dto.Club.IsPublic || dto.Club.Members.Any(x => x.UserID == userId))
			{
				return mapper.Map<ClubDiscussion>(dto);
			}
			return null;
		}
	}
}
