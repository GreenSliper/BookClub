using DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public interface IClubMemberService
	{
		Task<bool> JoinClub(int clubId, string userId);
		Task<bool> LeaveClub(int clubId, string userId);
		Task<IEnumerable<ClubInvite>> GetUserReceivedInvites(string userId);
		Task<bool> TrySendInvite(ClubInvite invite, ModelStateDictionary modelState, string inviterID);
		Task<bool> TryAcceptInvite(int clubId, string userId);
		Task<bool> TryDeclineInvite(int clubId, string userId);
		Task<ModelActionRequestResult<ClubMember>> ManageMember(string managerId, string targetUserId, int clubId);
		Task<bool> TryUpdateMember(ClubMember member, ModelStateDictionary modelState, string managerId);
	}
}
