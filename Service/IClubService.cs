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
		Task<IEnumerable<Club>> GetUserClubs(string userId);
		Task<bool> TryInsertClub(Club club, ModelStateDictionary modelState, string userId);
		public Task<IEnumerable<Club>> GetUserManagedClubs(string userId);
	}
}
