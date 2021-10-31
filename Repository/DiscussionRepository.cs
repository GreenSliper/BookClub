using DAL.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class DiscussionRepository<ContextT> : CrudRepository<ContextT, ClubDiscussion, int>
		where ContextT : DbContext
	{
		public DiscussionRepository(ContextT context) : base(context) { }
		public override async Task<ClubDiscussion> Get(int id)
		{
			return await entities.FirstOrDefaultAsync(x => x.ID == id);
		}
	}
}
