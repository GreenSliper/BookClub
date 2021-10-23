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
	}
}
