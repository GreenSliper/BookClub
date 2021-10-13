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
		public IActionResult MyClubs()
		{
			var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return View(clubService.GetUserClubs(userid));
		}
		
		[Authorize]
		public IActionResult Managed()
		{
			return View();
		}

		[Authorize]
		public IActionResult Create()
		{
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
