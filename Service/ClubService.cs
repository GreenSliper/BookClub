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
		public ClubService(IRepository<ReaderUser, string> userRepos, IRepository<DAL.DTO.Club, int> clubRepos, IMapper mapper)
		{
			this.userRepos = userRepos;
			this.clubRepos = clubRepos;
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
				   where (int)m.PermissionLevel>(int)DAL.DTO.MemberPermissions.Reader || m.Club.Creator.Id == userId
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
			var dto = await clubRepos.Get(id);
			if(dto.IsPublic || dto.Members.FirstOrDefault(x=>x.UserID == userId)!=null)
				return mapper.Map<Club>(dto);
			return null;
		}

		public async Task<bool> CanUserManageClub(int clubId, string userId)
		{
			var club = await clubRepos.Get(clubId);
			if (club.Creator?.Id == userId)
				return true;
			DAL.DTO.MemberPermissions? perm;
			if ((perm = club.Members.FirstOrDefault(x => x.UserID == userId)?.PermissionLevel) != null)
			{
				return (int)perm > (int)DAL.DTO.MemberPermissions.Reader;
			}
			return false;
		}

		public async Task<bool> TryUpdateClub(Club club, ModelStateDictionary modelState)
		{
			if (modelState.IsValid && club.ID.HasValue)
			{
				var old = await clubRepos.Get(club.ID.Value);
				var updated = mapper.Map<DAL.DTO.Club>(club);
				await clubRepos.Update(old, updated);
			}
			return false;
		}
	}
}
