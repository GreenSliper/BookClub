using DAL.Data;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class InviteRepository<ContextT> : CrudRepository<ContextT, ClubInvite, (int clubId, string receiverId)>,
		IExpirableRepos<ClubInvite, (int clubId, string receiverId)>
		where ContextT : ApplicationDbContext
	{
		public InviteRepository(ContextT context) : base(context) { }

		async Task<ClubInvite> GetOrRemoveExpired(ClubInvite invite)
		{
			if (invite.IsExpired)
			{
				await Delete(invite);
				return null;
			}
			return invite;
		}

		async IAsyncEnumerable<ClubInvite> GetCollectionRemoveExpired(IEnumerable<ClubInvite> source)
		{
			foreach (var inv in source)
			{
				var processed = await GetOrRemoveExpired(inv);
				if (processed != null)
					yield return processed;
			} 
		}
		public override async Task<IEnumerable<ClubInvite>> GetAll()
		{
			return await GetCollectionRemoveExpired(await base.GetAll()).ToListAsync();
		}
		public override async Task<ClubInvite> Get((int clubId, string receiverId) id)
		{
			return await GetOrRemoveExpired(
				await entities.FirstOrDefaultAsync(x => x.ClubID == id.clubId && x.ReceiverID == id.receiverId));
		}

		public async Task<IEnumerable<ClubInvite>> GetNotExpired(IEnumerable<ClubInvite> invites)
		{
			return await GetCollectionRemoveExpired(invites).ToListAsync();
		}

		public async Task<ClubInvite> GetNotExpired(ClubInvite invite)
		{
			return await GetOrRemoveExpired(invite);
		}
	}
}
