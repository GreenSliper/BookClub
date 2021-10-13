using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
	public class BookRepository<ContextT> : CrudRepository<ContextT, Book> where ContextT: DbContext
	{
		public BookRepository(ContextT context) : base(context) { }

		public async override Task<Book> Get(int id)
		{
			return await entities.FirstOrDefaultAsync(b => b.ID == id);
		}
	}
}
