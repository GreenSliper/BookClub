using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
	public class BookRepository<ContextT> : CrudRepository<ContextT, Book, int> where ContextT: DbContext
	{
		public BookRepository(ContextT context) : base(context) { }

		public override async Task<Book> Get(int id)
		{
			return await entities.FirstOrDefaultAsync(b => b.ID == id);
		}
	}
}
