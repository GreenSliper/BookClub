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
			if (!(await accessService.CanUserManageClub(disc.Club, userId)).Success)
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
			DAL.DTO.Club club = await clubRepos.Get(clubId);
			if (club == null || !(await accessService.CanUserManageClub(club, userId)).Success)
				return new ModelActionRequestResult<ClubDiscussion>(false);
			ReaderUser user = await userRepos.Get(userId);
			discussion.Creator = user;
			var dto = mapper.Map<DAL.DTO.ClubDiscussion>(discussion);
			club.Discussions.Add(dto);
			await clubRepos.Update(club);
			//ID will be auto-updated
			return new ModelActionRequestResult<ClubDiscussion>(true, mapper.Map<ClubDiscussion>(dto));
		}

		public async Task<ModelActionRequestResult<ClubDiscussion>> TryGetDiscussion(int discId, string userId)
		{
			var dto = await discussionRepos.Get(discId);
			if ((await accessService.CanUserViewClub(dto.Club, userId)).Success)
				return new ModelActionRequestResult<ClubDiscussion>(true, mapper.Map<ClubDiscussion>(dto));
			return new ModelActionRequestResult<ClubDiscussion>(false);
		}

		public async Task<bool> TryRefreshBooksPriorities(IEnumerable<ClubDiscussionBook> discussionBooks, 
			int discId, string userId)
		{
			var dto = await discussionRepos.Get(discId);
			if (!(await accessService.CanUserManageClub(dto.Club, userId)).Success)
				return false;
			RefreshBooksPriorities(dto.Books, discussionBooks);
			await discussionRepos.Update(dto);
			return true;
		}

		void RefreshBooksPriorities(IEnumerable<DAL.DTO.ClubDiscussionBook> old, IEnumerable<ClubDiscussionBook> updated)
		{
			foreach (var bk in updated)
			{
				var edited = old.FirstOrDefault(x => x.BookID == bk.Book.ID);
				if (edited != null)
					edited.Priority = bk.Priority;
			}
		}

		public async Task<bool> TryUpdateDiscussion(ClubDiscussion discussion,
			ModelStateDictionary modelState, IEnumerable<int> removedBookIds, string userId)
		{
			if (!modelState.IsValid)
				return false;
			var dto = await discussionRepos.Get(discussion.ID);
			if (!(await accessService.CanUserManageClub(dto.Club, userId)).Success)
				return false;
			
			//update fields: manual, because some fields are not affected
			dto.Name = discussion.Name;
			dto.Description = discussion.Description;
			dto.Time = discussion.Time;
			//remove from list
			if (removedBookIds != null)
				foreach (var id in removedBookIds)
					dto.Books.Remove(dto.Books.FirstOrDefault(x => x.BookID == id));
			RefreshBooksPriorities(dto.Books, discussion.Books);
			
			await discussionRepos.Update(dto);
			return true;
		}
	}
}
