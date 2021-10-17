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
	}
}
