using DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public interface IDiscussionService
	{
		Task<ModelActionRequestResult<ClubDiscussion>> TryAddDiscussion
			(ClubDiscussion discussion, ModelStateDictionary modelState, int clubId, string userId);
		Task<ClubDiscussion> TryGetDiscussion(int discId, string userId);
		Task<bool> TryAddBooks(IEnumerable<int> bookIDs, int discId, string userId);
	}
}
