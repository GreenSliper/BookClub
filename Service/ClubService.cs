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
			return from m in (await userRepos.Get(userId)).Memberships
				   orderby m.LastVisitTime descending
				   select mapper.Map<Club>(m.Club);
		}

		public async Task<IEnumerable<Club>> GetUserManagedClubs(string userId)
		{
			return from m in (await userRepos.Get(userId)).Memberships
				   where accessService.CanUserManageClub(m)
				   orderby m.LastVisitTime descending
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
					User = dto.Creator,
					PermissionLevel = DAL.DTO.MemberPermissions.Creator
				});
				await clubRepos.Insert(dto);
				return true;
			}
			return false;
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
			var club = await clubRepos.Get(clubId);
			if (!(await accessService.GetClub(club, userId, MemberActions.ManageClub)).Success)
				return false;
			var user = await userRepos.Get(userId);
			foreach (var bookId in bookIds)
				club.Books.Add(new DAL.DTO.ClubBook()
				{
					AddedByUser = user,
					Club = club,
					BookID = bookId,
					AddedTime = DateTime.Now
				});
			await clubRepos.Update(club);
			return true;
		}

		public async Task<IEnumerable<Club>> GetPublicClubs()
		{
			var clubs = await clubRepos.GetAll();
			return from c in clubs
				   where c.IsPublic
				   select mapper.Map<Club>(c);
		}

		public async Task<ModelAccessResult<Club, Ban, AccessErrors>> GetClub(int id, string userId, MemberActions targetAction)
		{
			var club = await clubRepos.Get(id);
			var accessResult = await accessService.GetClub(club, userId, targetAction);
			if (!accessResult.Success)
				return accessResult.Map<Club, Ban>(mapper);
			club.Members.FirstOrDefault(x => x.UserID == userId).LastVisitTime = DateTime.Now;
			await clubRepos.SaveChanges();
			return new ModelAccessResult<Club, Ban, AccessErrors>(mapper.Map<Club>(club));
		}
	}
}
