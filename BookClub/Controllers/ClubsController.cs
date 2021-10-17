using BookClub.Models;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookClub.Controllers
{
	public class ClubsController : Controller
	{
		private readonly IClubService clubService;
		private readonly IBookService bookService;
		public ClubsController(IClubService clubService, IBookService bookService)
		{
			this.clubService = clubService;
			this.bookService = bookService;
		}
		public IActionResult Index()
		{
			return View();
		}

		[Authorize]
		public async Task<IActionResult> MyClubs()
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return View(await clubService.GetUserClubs(userid));
		}
		
		[Authorize]
		public async Task<IActionResult> Managed()
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return View(await clubService.GetUserManagedClubs(userid));
		}
		
		[Authorize]
		public IActionResult Create()
		{
			return View();
		}
		
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromForm] Club club)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (await clubService.TryInsertClub(club, ModelState, userid))
			{
				return RedirectToAction("MyClubs");
			}
			return View();
		}

		[Authorize]
		public IActionResult UpcomingDiscussions()
		{
			return View();
		}

		public IActionResult Browse()
		{
			return View();
		}

		public async Task<IActionResult> ViewClub(int id)
		{
			string userid = null;
			if (User.Identity.IsAuthenticated)
			{
				userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if ((await clubService.CanUserManageClub(id, userid)).successful)
					return RedirectToAction("Manage", new { id });
			}
			var club = await clubService.GetClubView(id, userid);
			if (club != null)
				return View(club);
			else //TODO to error not found / no access page
				return RedirectToAction("Index");
		}
		
		[Authorize]
		public async Task<IActionResult> Manage(int id)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var request = await clubService.CanUserManageClub(id, userid);
			if (request.successful)
				return View(request.requestedModel);
			else
				return RedirectToAction("ViewClub", new { id });
		}
		
		[Authorize]
		public async Task<IActionResult> Edit(int id)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var request = await clubService.CanUserManageClub(id, userid);
			if (request.successful)
				return View(request.requestedModel);
			else
				return RedirectToAction("ViewClub", new { id });
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromForm] Club club)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (club.ID.HasValue && (await clubService.CanUserManageClub(club.ID.Value, userid)).successful)
			{
				if (await clubService.TryUpdateClub(club, ModelState))
					return RedirectToAction("ViewClub", new { club.ID });
				else
					; //TODO err page
			}
			return RedirectToAction("ViewClub", new { club.ID });
		}

		[Authorize]
		public async Task<IActionResult> AddBooks(int id)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var request = await clubService.CanUserManageClub(id, userid);
			if (request.successful) {
				var allBooks = await bookService.GetAllBooks();
				var targetBooks = from b in allBooks
								  where !request.requestedModel.Books.Any(x => x.Book.ID == b.ID)
								  select b;
				BookPickerModel bpm = new BookPickerModel(id, targetBooks);
				ViewBag.BookList = targetBooks;
				ViewBag.Club = request.requestedModel;
				return View(bpm);
			} else
				return RedirectToAction("ViewClub", new { id });
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddBooks([FromForm] BookPickerModel bpm)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var added = new List<int>();
			for (int i = 0; i < bpm.ids.Count; i++)
				if (bpm.added[i])
					added.Add(bpm.ids[i]);
			await clubService.TryAddBooks(added, bpm.entityID, userid);
			return RedirectToAction("ViewClub", new { id = bpm.entityID });
		}
	}
}
