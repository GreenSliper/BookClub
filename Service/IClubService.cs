using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public interface IClubService
	{
		Task<ModelAccessResult<Club, Ban, AccessErrors>> GetClub(int id, string userId, MemberActions targetAction);
		Task<bool> TryInsertClub(Club club, ModelStateDictionary modelState, string userId);
		Task<bool> TryUpdateClub(Club club, ModelStateDictionary modelState);
		Task<IEnumerable<Club>> GetPublicClubs();
		Task<IEnumerable<Club>> GetUserClubs(string userId);
		Task<IEnumerable<Club>> GetUserManagedClubs(string userId);
		Task<bool> TryAddBooks(IEnumerable<int> bookIds, int clubId, string userId);
	}
}
