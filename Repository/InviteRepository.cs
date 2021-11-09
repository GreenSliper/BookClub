using DAL.Data;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class InviteRepository<ContextT> : ExpirableCrudRepos<ContextT, ClubInvite, (int clubId, string receiverId)>,
		IExpirableRepos<ClubInvite, (int clubId, string receiverId)>
		where ContextT : Microsoft.EntityFrameworkCore.DbContext
	{
		public InviteRepository(ContextT context) : base(context) { }

		public override async Task<ClubInvite> Get((int clubId, string receiverId) id)
		{
			return await GetOrRemoveExpired(
				await entities.FirstOrDefaultAsync(x => x.ClubID == id.clubId && x.ReceiverID == id.receiverId));
		}
	}
}