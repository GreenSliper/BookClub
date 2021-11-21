using DAL.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class ReadBookRepository<ContextT> : CrudRepository<ContextT, ReadBook, (int bookId, string userId)>
		where ContextT : DbContext
	{
		public ReadBookRepository(ContextT context) : base(context) { }
		public override async Task<ReadBook> Get((int bookId, string userId) id)
		{
			return await entities.FirstOrDefaultAsync(x => x.BookID == id.bookId && x.ReaderID == id.userId);
		}
	}
}
