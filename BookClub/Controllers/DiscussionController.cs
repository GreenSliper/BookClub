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
		public DiscussionController(IClubService clubService)
		{
			this.clubService = clubService;
		}
		[Authorize]
		public async Task<IActionResult> Add(int id)
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var request = await clubService.CanUserManageClub(id, userid);
			if (request.successful)
			{
				return View(new ClubDiscussion {Club = request.requestedClub});
			}
			//access denied or club not found
			//redirect back to club
			return RedirectToAction("ViewClub", "Clubs", new { id });
		}
	}
}
