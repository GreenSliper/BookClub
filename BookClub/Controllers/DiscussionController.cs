﻿using BookClub.Models;
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
			int.TryParse(TempData["ClubId"].ToString(), out int clubId);
			if(discussion != null)
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
			return View(new BookPickerModel(id, disc.Club.Books));
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
			await discussionService.TryAddBooks(added, bpm.entityID, userid);
			return RedirectToAction("View", new { id = bpm.entityID });
		}
	}
}
