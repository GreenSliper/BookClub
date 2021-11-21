using DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public interface IBookService
	{
		Task<ModelActionRequestResult<Book>> TryInsertBook(Book book, ModelStateDictionary modelState, string userId);
		Task<IEnumerable<Book>> GetAllBooks();
		Task<Book> GetBook(int id);
		Task<ReadBook> GetOrCreateReadBook(int id, string userId);
		Task<bool> IsBookRead(int id, string userId);
		Task<bool> InsertOrUpdateReadBook(ReadBook readBook, ModelStateDictionary modelState, string userId);
	}
}
