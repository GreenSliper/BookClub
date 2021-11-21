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

		string UserId { get => User.FindFirstValue(ClaimTypes.NameIdentifier); }

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
			var result = await bookService.TryInsertBook(book, ModelState, UserId);
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
		public async Task<IActionResult> View(int id)
		{
			ViewBag.IsBookRead = false;
			if (User.Identity.IsAuthenticated)
				ViewBag.IsBookRead = await bookService.IsBookRead(id, UserId);
			return View(await bookService.GetBook(id));
		}

		[Authorize]
		public async Task<IActionResult> MarkRead(int id)
		{
			return View(await bookService.GetOrCreateReadBook(id, UserId));
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> MarkRead([FromForm] ReadBook readBook)
		{
			if (await bookService.InsertOrUpdateReadBook(readBook, ModelState, UserId))
				return RedirectToAction("View", new { id = readBook.BookID });
			return View(readBook);
		}
	}
}
