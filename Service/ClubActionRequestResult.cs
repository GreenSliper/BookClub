using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class ClubActionRequestResult
	{
		public readonly bool successful;
		public readonly Club requestedClub;

		public ClubActionRequestResult(bool successful, Club requestedClub = null)
		{
			this.successful = successful;
			this.requestedClub = requestedClub;
		}
	}
}
