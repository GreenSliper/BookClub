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
		private readonly IMapper mapper;
		public BookService(IRepository<ReaderUser, string> userRepos, IRepository<DAL.DTO.Book, int> clubRepos, IMapper mapper)
		{
			this.userRepos = userRepos;
			this.bookRepos = clubRepos;
			this.mapper = mapper;
		}

		public async Task<IEnumerable<Book>> GetAllBooks()
		{
			return mapper.Map<IEnumerable<DAL.DTO.Book>, IEnumerable<Book>>(await bookRepos.GetAll());
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
