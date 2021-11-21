using BookClub.Models;
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
	public class DiscussionController : Controller
	{
		private readonly IClubService clubService;
		private readonly IDiscussionService discussionService;

		public DiscussionController(IClubService clubService, IDiscussionService discussionService)
		{
			this.clubService = clubService;
			this.discussionService = discussionService;
		}
		[Authorize]
		public async Task<IActionResult> Add(int id)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var request = await clubService.GetClub(id, userid, MemberActions.ManageClub);
			if (request.Success)
			{
				ViewBag.ClubId = id;
				return View(new ClubDiscussion { Club = request.requestedModel });
			}
			//access denied or club not found
			//redirect back to club
			return RedirectToAction("ViewClub", "Clubs", new { id });
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add([FromForm] ClubDiscussion discussion)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (int.TryParse(TempData["ClubId"].ToString(), out int clubId))
				if (discussion != null)
					if ((await clubService.GetClub(clubId, userid, MemberActions.ManageClub)).Success)
					{
						var result = await discussionService.TryAddDiscussion(discussion, ModelState, clubId, userid);
						if (result.successful)
							return RedirectToAction("AddBooks", new { result.requestedModel.ID });
						else
							; //TODO err page
					}
			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		public async Task<IActionResult> AddBooks(int id)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var request = await discussionService.TryGetDiscussion(id, userid);
			if (request.successful)
				ViewBag.BookList = from b in request.requestedModel.Club.Books select b.Book;
			return View(request.requestedModel);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddBooks([FromForm] ClubDiscussion disc)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var added = TempData["SelectedBookList"] as int[];
			TempData.Remove("SelectedBookList");
			await discussionService.TryAddBooks(added, disc.ID, userid);
			return RedirectToAction("ManageBooks", new { id = disc.ID });
		}

		[Authorize]
		public async Task<IActionResult> ManageBooks(int id)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var request = await discussionService.TryGetDiscussion(id, userid);
			return View(request.requestedModel);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ManageBooks([FromForm] List<ClubDiscussionBook> books)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (books == null || books.Count == 0)
				;//TODO ERR PAGE
			var discId = books[0].Discussion.ID;
			await discussionService.TryRefreshBooksPriorities(books, discId, userid);
			return RedirectToAction("ViewDiscussion", new { id = discId });
		}

		public async Task<IActionResult> ViewDiscussion(int id)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var request = await discussionService.TryGetDiscussion(id, userid);
			if (request.successful)
			{
				ViewBag.UserCanEdit = (await clubService.GetClub(request.requestedModel.Club.ID.Value, userid, MemberActions.ManageClub))
					.Success;
				return View(request.requestedModel);
			}
			else
				;//TODO ERR PAGE
			return null;
		}

		[Authorize]
		public async Task<IActionResult> Edit(int id)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var request = await discussionService.TryGetDiscussion(id, userid);
			if (request.successful)
			{
				ViewBag.UserCanEdit = await clubService.GetClub(request.requestedModel.Club.ID.Value, userid, MemberActions.ManageClub);
				return View(request.requestedModel);
			}
			else
				return RedirectToAction("ViewDiscussion", new { id });
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromForm] ClubDiscussion discussion)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var removed = TempData["SelectedBookList"] as int[];
			TempData.Remove("SelectedBookList");
			if (await discussionService.TryUpdateDiscussion(discussion, ModelState, removed, userid))
				return RedirectToAction("ViewDiscussion", new { id = discussion.ID });
			return View();
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditAndAddBooks([FromForm] ClubDiscussion discussion)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var removed = TempData["SelectedBookList"] as int[];
			TempData.Remove("SelectedBookList");
			if (await discussionService.TryUpdateDiscussion(discussion, ModelState, removed, userid))
				return RedirectToAction("AddBooksToExisting", new { id = discussion.ID });
			return RedirectToAction("Edit", new { discussion });
		}

		[Authorize]
		public async Task<IActionResult> AddBooksToExisting(int id)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var request = await discussionService.TryGetDiscussion(id, userid);
			if (request.successful)
			{
				var disc = request.requestedModel;
				ViewBag.BookList = from bk in disc.Club.Books
								   where !disc.Books.Any(x=>x.Book.ID == bk.Book.ID)
								   select bk.Book;
				return View("AddBooks", request.requestedModel);
			}
			else
				return RedirectToAction("ViewDiscussion", new { id });
		}
	}
}
