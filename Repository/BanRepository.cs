using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class BanRepository<ContextT> : ExpirableCrudRepos<ContextT, Ban, (int clubId, string bannedUserID)>,
		IExpirableRepos<Ban, (int clubId, string bannedUserID)>
		where ContextT : Microsoft.EntityFrameworkCore.DbContext
	{
		public BanRepository(ContextT context) : base(context) { }
		public override async Task<Ban> Get((int clubId, string bannedUserID) id)
		{
			return await GetOrRemoveExpired(
				await entities.FirstOrDefaultAsync(x => x.ClubID == id.clubId && x.BannedUserID == id.bannedUserID));
		}
	}
}
