using BookClub.Models;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Helpers;
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
		private readonly IClubMemberService memberService;
		private readonly IBookService bookService;
		public ClubsController(IClubService clubService, IBookService bookService, IClubMemberService memberService)
		{
			this.clubService = clubService;
			this.bookService = bookService;
			this.memberService = memberService;
		}

		string UserId { get => User.FindFirstValue(ClaimTypes.NameIdentifier); }

		public IActionResult Index()
		{
			return View();
		}

		[Authorize]
		public async Task<IActionResult> MyClubs()
		{
			return View(await clubService.GetUserClubs(UserId));
		}
		
		[Authorize]
		public async Task<IActionResult> Managed()
		{
			return View(await clubService.GetUserManagedClubs(UserId));
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
			if (await clubService.TryInsertClub(club, ModelState, UserId))
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

		public async Task<IActionResult> Browse()
		{
			var clubs = await clubService.GetPublicClubs();
			return View(clubs.OrderByDescending(x=>x.Members.Count));
		}

		public async Task<IActionResult> ViewClub(int id)
		{
			if (User.Identity.IsAuthenticated)
			{
				if ((await clubService.CanUserManageClub(id, UserId)).successful)
					return RedirectToAction("Manage", new { id });
			}
			var club = await clubService.GetClubView(id, UserId);
			if (club != null)
			{
				ViewBag.IsUserMember = UserId != null && club.Members.Any(x => x.User.Id == UserId);
				return View(club);
			}
			else //TODO to error not found / no access page
				return RedirectToAction("Index");
		}
		
		[Authorize]
		public async Task<IActionResult> Manage(int id)
		{
			var request = await clubService.CanUserManageClub(id, UserId);
			if (request.successful)
				return View(request.requestedModel);
			else
				return RedirectToAction("ViewClub", new { id });
		}
		
		[Authorize]
		public async Task<IActionResult> Edit(int id)
		{
			var request = await clubService.CanUserManageClub(id, UserId);
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
			if (club.ID.HasValue && (await clubService.CanUserManageClub(club.ID.Value, UserId)).successful)
			{
				if (await clubService.TryUpdateClub(club, ModelState))
					return RedirectToAction("ViewClub", new { club.ID });
				else
					return View(club);
			}
			return RedirectToAction("ViewClub", new { club.ID });
		}

		[Authorize]
		public async Task<IActionResult> AddBooks(int id)
		{
			var request = await clubService.CanUserManageClub(id, UserId);
			if (request.successful) {
				var allBooks = await bookService.GetAllBooks();
				var targetBooks = from b in allBooks
								  where !request.requestedModel.Books.Any(x => x.Book.ID == b.ID)
								  select b;
				ViewBag.BookList = targetBooks;
				return View(request.requestedModel);
			} else
				return RedirectToAction("ViewClub", new { id });
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddBooksConfirm(Club club)
		{
			//magic is that the list is auto-converted to array
			var idList = TempData["SelectedBookList"] as int[];
			TempData.Remove("SelectedBookList");
			await clubService.TryAddBooks(idList, club.ID.Value, UserId);
			return RedirectToAction("ViewClub", new { id = club.ID.Value });
		}
	}
}
