using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class UserRepository<ContextT> : CrudRepository<ContextT, ReaderUser, string> where ContextT:DbContext
	{
		public UserRepository(ContextT context) : base(context) { }
		public override async Task<ReaderUser> Get(string id)
		{
			return await entities.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
