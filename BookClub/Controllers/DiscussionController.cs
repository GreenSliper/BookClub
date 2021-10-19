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
			var request = await clubService.CanUserManageClub(id, userid);
			if (request.successful)
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
					if ((await clubService.CanUserManageClub(clubId, userid)).successful)
					{
						var result = await discussionService.TryAddDiscussion(discussion, ModelState, clubId, userid);
						if (result.successful)
							return RedirectToAction("AddBooks", new { result.requestedModel.ID });
						else
							; //TODO err page
					}
			return RedirectToAction("Index", "Home" );
		}

		[Authorize]
		public async Task<IActionResult> AddBooks(int id)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var disc = await discussionService.TryGetDiscussion(id, userid);
			if (disc != null)
				ViewBag.BookList = from b in disc.Club.Books select b.Book;
			return View(disc);
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
			var disc = await discussionService.TryGetDiscussion(id, userid);
			return View(disc);
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
			var disc = await discussionService.TryGetDiscussion(id, userid);
			if (disc != null) {
				ViewBag.UserCanEdit = (await clubService.CanUserManageClub(disc.Club.ID.Value, userid))
					.successful;
				return View(disc);
			}
			else
				;//TODO ERR PAGE
			return null;
		}

		[Authorize]
		public async Task<IActionResult> Edit(int id) 
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var disc = await discussionService.TryGetDiscussion(id, userid);
			if (disc != null)
			{
				ViewBag.UserCanEdit = await clubService.CanUserManageClub(disc.Club.ID.Value, userid);
				return View(disc);
			}
			else
				return RedirectToAction("ViewDiscussion", new { id });
		}
		
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromForm]ClubDiscussion discussion)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var removed = TempData["SelectedBookList"] as int[];
			TempData.Remove("SelectedBookList");
			if (await discussionService.TryUpdateDiscussion(discussion, ModelState, removed, userid))
				return RedirectToAction("ViewDiscussion", new { id = discussion.ID });
			return View();
		}
	}
}
