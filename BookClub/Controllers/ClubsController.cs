using BookClub.Models;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Web.Helpers;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookClub.Controllers.Services;

namespace BookClub.Controllers
{
	public class ClubsController : Controller
	{
		private readonly IClubService clubService;
		private readonly IClubMemberService memberService;
		private readonly IBookService bookService;

		private readonly IErrorDisplayHandler<ModelAccessResult<Club, Ban, AccessErrors>, AccessErrors> viewDisplayer;
		private readonly IErrorDisplayHandler<ModelAccessResult<Club, Ban, AccessErrors>, AccessErrors> manageDisplayer;
		public ClubsController(IClubService clubService, IBookService bookService, IClubMemberService memberService)
		{
			this.clubService = clubService;
			this.bookService = bookService;
			this.memberService = memberService;

			//configure service request error actions
			viewDisplayer = new AccessErrorHandler<Club, Ban, AccessErrors>(this);
			manageDisplayer = new AccessErrorHandler<Club, Ban, AccessErrors>(this);
			
			var commonHandlers = new Dictionary<AccessErrors, IRequestErrorHandler>()
			{
				{ AccessErrors.NoAccess, new ErrorHandler(viewName:"Index") },
				{ AccessErrors.NotFound, new ErrorHandler(viewName:"Index") },
				{ AccessErrors.Banned, new ErrorHandler("Banned") }
			};
			viewDisplayer.SetErrorHandlers(commonHandlers);
			manageDisplayer.SetErrorHandlers(commonHandlers);
			manageDisplayer.SetHandler(AccessErrors.NotPermitted, 
				new ErrorHandlerRedirect("ViewClub", "Clubs", "id"));
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
			return View(clubs.OrderByDescending(x => x.Members.Count));
		}

		public async Task<IActionResult> ViewClub(int id)
		{
			if (User.Identity.IsAuthenticated)
			{
				var manageRequest = await clubService.GetUserManagedClub(id, UserId);
				if (manageRequest.Success)
					return await viewDisplayer.GetResult(manageRequest, "Manage");
			}
			var viewRequest = await clubService.GetClubView(id, UserId);
			if (viewRequest.Success)
				ViewBag.IsUserMember = viewRequest.requestedModel.Members.Any(x => x.User.Id == UserId);
			return await viewDisplayer.GetResult(viewRequest, View().ViewName);
		}
		
		[Authorize]
		public async Task<IActionResult> Manage(int id)
		{
			var manageRequest = await clubService.GetUserManagedClub(id, UserId);
			return await manageDisplayer.GetResult(manageRequest, View().ViewName);
		}
		
		[Authorize]
		public async Task<IActionResult> Edit(int id)
		{
			var manageRequest = await clubService.GetUserManagedClub(id, UserId);
			return await manageDisplayer.GetResult(manageRequest, View().ViewName);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromForm] Club club)
		{
			if (club.ID.HasValue)
			{
				var manageRequest = await clubService.GetUserManagedClub(club.ID.Value, UserId);
				if(!manageRequest.Success)
					return await manageDisplayer.GetErrorResult(manageRequest);
				if (await clubService.TryUpdateClub(club, ModelState))
					return RedirectToAction("ViewClub", new { club.ID });
				else
					return View(club);
			}
			return await manageDisplayer.GetErrorResult(new ModelAccessResult<Club, Ban, AccessErrors>(AccessErrors.NotFound));
		}

		[Authorize]
		public async Task<IActionResult> AddBooks(int id)
		{
			var manageRequest = await clubService.GetUserManagedClub(id, UserId);
			if (manageRequest.Success) {
				var allBooks = await bookService.GetAllBooks();
				var targetBooks = from b in allBooks
								  where !manageRequest.requestedModel.Books.Any(x => x.Book.ID == b.ID)
								  select b;
				ViewBag.BookList = targetBooks;
				return View(manageRequest.requestedModel);
			} else
				return await manageDisplayer.GetErrorResult(manageRequest);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddBooksConfirm(Club club)
		{
			var manageRequest = await clubService.GetUserManagedClub(club.ID.Value, UserId);
			if (!manageRequest.Success)
				return await manageDisplayer.GetErrorResult(manageRequest);
			//magic is that the list is auto-converted to array
			var idList = TempData["SelectedBookList"] as int[];
			TempData.Remove("SelectedBookList");
			await clubService.TryAddBooks(idList, club.ID.Value, UserId);
			return RedirectToAction("ViewClub", new { id = club.ID.Value });
		}
	}
}
