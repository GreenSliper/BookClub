using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookClub.Controllers
{
	public class BookController : Controller
	{
		private readonly IBookService bookService;
		public BookController(IBookService bookService)
		{
			this.bookService = bookService;
		}

		[Authorize]
		public IActionResult Add()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add([FromForm] Book book)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await bookService.TryInsertBook(book, ModelState, userid);
			if (result.successful)
				return View("AddSuccess", result.requestedModel);
			return View();
		}

		public async Task<IActionResult> Library()
		{
			return View(await bookService.GetAllBooks());
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
