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
		public ClubsController(IClubService clubService)
		{
			this.clubService = clubService;
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
	}
}
