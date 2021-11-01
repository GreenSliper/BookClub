using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Service
{
	public class ClubService : IClubService
	{
		private readonly IRepository<ReaderUser, string> userRepos;
		private readonly IRepository<DAL.DTO.Club, int> clubRepos;
		private readonly IMapper mapper;
		private readonly IAccessService accessService;
		public ClubService(IRepository<ReaderUser, string> userRepos, 
			IRepository<DAL.DTO.Club, int> clubRepos, IAccessService accessService, IMapper mapper)
		{
			this.userRepos = userRepos;
			this.clubRepos = clubRepos;
			this.accessService = accessService;
			this.mapper = mapper;
		}
		public async Task<IEnumerable<Club>> GetUserClubs(string userId)
		{
			//select clubs from membership and map from DTO to model
			return from m in (await userRepos.Get(userId)).Memberships
				   orderby m.LastVisitTime descending
				   select mapper.Map<Club>(m.Club);
		}

		public async Task<IEnumerable<Club>> GetUserManagedClubs(string userId)
		{
			//logics: creator/manager of the club is always the member
			return from m in (await userRepos.Get(userId)).Memberships
				   where (int)m.PermissionLevel>=(int)accessService.MinimalToManage || m.Club.Creator.Id == userId
				   select mapper.Map<Club>(m.Club);
		}

		public async Task<bool> TryInsertClub(Club club, ModelStateDictionary modelState, string userId)
		{
			if (modelState.IsValid)
			{
				var dto = mapper.Map<DAL.DTO.Club>(club);
				dto.Creator = await userRepos.Get(userId);
				dto.Members.Add(new DAL.DTO.ClubMember {
					Club = dto, 
					LastVisitTime = DateTime.Now,
					User = dto.Creator
				});
				await clubRepos.Insert(dto);
				return true;
			}
			return false;
		}

		public async Task<Club> GetClubView(int id, string userId)
		{
			var request = await accessService.CanUserViewClub(id, userId);
			if(request.successful)
				return mapper.Map<Club>(request.requestedModel);
			return null;
		}

		public async Task<ModelActionRequestResult<Club>> CanUserManageClub(int clubId, string userId)
		{
			var result = await accessService.CanUserManageClub(clubId, userId);
			return new ModelActionRequestResult<Club>(result.successful, mapper.Map<Club>(result.requestedModel));
		}

		public async Task<bool> TryUpdateClub(Club club, ModelStateDictionary modelState)
		{
			if (modelState.IsValid && club.ID.HasValue)
			{
				var old = await clubRepos.Get(club.ID.Value);
				var updated = mapper.Map<DAL.DTO.Club>(club);
				if (updated.AvatarImage == null && old.AvatarImage != null)
					updated.AvatarImage = old.AvatarImage;
				await clubRepos.Update(old, updated);
				return true;
			}
			return false;
		}

		public async Task<bool> TryAddBooks(IEnumerable<int> bookIds, int clubId, string userId)
		{
			var request = await accessService.CanUserManageClub(clubId, userId);
			if (!request.successful || request.requestedModel == null)
				return false;
			var user = await userRepos.Get(userId);
			var dto = request.requestedModel;
			foreach (var bookId in bookIds)
				dto.Books.Add(new DAL.DTO.ClubBook() 
				{ 
					AddedByUser = user,
					Club = dto,
					BookID = bookId,
					AddedTime = DateTime.Now
				});
			await clubRepos.Update(dto);
			return true;
		}

		public async Task<IEnumerable<Club>> GetPublicClubs()
		{
			var clubs = await clubRepos.GetAll();
			return from c in clubs
				   where c.IsPublic
				   select mapper.Map<Club>(c);
		}
	}
}
