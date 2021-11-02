using DAL.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class MemberRepos<ContextT> : CrudRepository<ContextT, ClubMember, (int clubId, string userId)>
		where ContextT : DbContext
	{
		public MemberRepos(ContextT context) : base(context) { }

		public override async Task<ClubMember> Get((int clubId, string userId) id)
		{
			return await entities.FirstOrDefaultAsync(b => b.ClubID == id.clubId && b.UserID==id.userId);
		}
	}
}
