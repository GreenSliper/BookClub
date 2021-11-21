using AutoMapper;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class BookService : IBookService
	{
		private readonly IRepository<ReaderUser, string> userRepos;
		private readonly IRepository<DAL.DTO.Book, int> bookRepos;
		IRepository<DAL.DTO.ReadBook, (int bookId, string userId)> readBookRepos;
		private readonly IMapper mapper;
		public BookService(IRepository<ReaderUser, string> userRepos, IRepository<DAL.DTO.Book, int> clubRepos,
			IRepository<DAL.DTO.ReadBook, (int bookId, string userId)> readBookRepos, IMapper mapper)
		{
			this.userRepos = userRepos;
			this.bookRepos = clubRepos;
			this.readBookRepos = readBookRepos;
			this.mapper = mapper;
		}

		public async Task<IEnumerable<Book>> GetAllBooks()
		{
			return mapper.Map<IEnumerable<DAL.DTO.Book>, IEnumerable<Book>>(await bookRepos.GetAll());
		}

		public async Task<Book> GetBook(int id)
		{
			return mapper.Map<Book>(await bookRepos.Get(id));
		}

		public async Task<ReadBook> GetOrCreateReadBook(int id, string userId)
		{
			var readBook = (await userRepos.Get(userId))?.ReadBooks.FirstOrDefault(x => x.BookID == id);
			if (readBook == null)
			{
				var book = await bookRepos.Get(id);
				if (book != null)
				{
					return new ReadBook()
					{
						BookID = book.ID,
						BookName = book.Name
					};
				}
			}
			return mapper.Map<ReadBook>(readBook);
		}

		public async Task<bool> InsertOrUpdateReadBook(ReadBook readBook, ModelStateDictionary modelState, string userId)
		{
			if (!modelState.IsValid)
				return false;
			var oldDto = (await userRepos.Get(userId))?.ReadBooks.FirstOrDefault(x => x.BookID == readBook.BookID);
			var newDto = mapper.Map<DAL.DTO.ReadBook>(readBook);
			newDto.ReaderID = userId;
			if (oldDto != null)
				await readBookRepos.Update(oldDto, newDto);
			else
				await readBookRepos.Insert(newDto);
			return true;
		}

		public async Task<bool> IsBookRead(int id, string userId)
		{
			return (await userRepos.Get(userId))?.ReadBooks.Any(x => x.BookID == id) == true;
		}

		public async Task<ModelActionRequestResult<Book>> TryInsertBook(Book book, ModelStateDictionary modelState, string userId)
		{
			if (!modelState.IsValid)
				return new ModelActionRequestResult<Book>(false);
			var dto = mapper.Map<DAL.DTO.Book>(book);
			var addedBy = await userRepos.Get(userId);
			if(addedBy == null)
				return new ModelActionRequestResult<Book>(false);
			dto.AddedByUser = addedBy;
			await bookRepos.Insert(dto);
			return new ModelActionRequestResult<Book>(true, mapper.Map<Book>(dto));
		}
	}
}
