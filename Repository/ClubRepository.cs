using DAL.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class ClubRepository<ContextT> : CrudRepository<ContextT, Club, int>
		where ContextT : DbContext
	{
		public ClubRepository(ContextT context) : base(context)	{	}

		public override Task Update(Club old, Club updated = null)
		{
			//related entity should be attached
			if (updated != null)
				old.AvatarImage = updated.AvatarImage;
			return base.Update(old, updated);
		}

		public override async Task<Club> Get(int id)
		{
			return await entities.FirstOrDefaultAsync(x => x.ID == id);
		}
	}
}
