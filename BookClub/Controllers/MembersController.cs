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
	public class MembersController : Controller
	{
		private readonly IClubService clubService;
		private readonly IClubMemberService memberService;
		public MembersController(IClubService clubService, IClubMemberService memberService)
		{
			this.clubService = clubService;
			this.memberService = memberService;
		}
		string UserId { get => User.FindFirstValue(ClaimTypes.NameIdentifier); }

		public async Task<IActionResult> Index(int id)
		{
			var request = await clubService.CanUserManageClub(id, UserId);
			if (request.successful)
			{
				return View(request.requestedModel);
			}
			return RedirectToAction("ViewClub", "Clubs", new { id });
		}

		[Authorize]
		public async Task<IActionResult> Join(int id)
		{
			if (await memberService.JoinClub(id, UserId))
				return RedirectToAction("ViewClub", "Clubs", new { id });
			//TODO: maybe add some err page
			return RedirectToAction("ViewClub", "Clubs", new { id });
		}

		[Authorize]
		public async Task<IActionResult> Leave(int id)
		{
			if (await memberService.LeaveClub(id, UserId))
				return RedirectToAction("ViewClub", "Clubs", new { id });
			//TODO: maybe add some err page
			return RedirectToAction("ViewClub", "Clubs", new { id });
		}

		[Authorize]
		public async Task<IActionResult> SendInvite(int id)
		{
			var request = await clubService.CanUserManageClub(id, UserId);
			if (request.successful)
			{
				return View(new ClubInvite()
				{
					ClubID = request.requestedModel.ID.Value,
					Club = request.requestedModel
				});
			}
			return RedirectToAction("ViewClub", "Clubs", new { id });
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SendInvite([FromForm] ClubInvite invite)
		{
			if (await memberService.TrySendInvite(invite, ModelState, UserId))
			{
				//TODO add success page
				return RedirectToAction("ViewClub", "Clubs", new { id = invite.ClubID });
			}
			invite.Club = await clubService.GetClubView(invite.ClubID, UserId);
			return View(invite);
		}

		[Authorize]
		public async Task<IActionResult> Invites()
		{
			var invites = await memberService.GetUserReceivedInvites(UserId);
			return View(invites);
		}

		[Authorize]
		public async Task<IActionResult> ViewInvite(int id)
		{
			var invite = (await memberService.GetUserReceivedInvites(UserId)).FirstOrDefault(x => x.ClubID == id);
			if (invite == null) //TODO error page
				return RedirectToAction("Invites");
			return View(invite);
		}

		[Authorize]
		public async Task<IActionResult> AcceptInvite(int id)
		{
			if (await memberService.TryAcceptInvite(id, UserId)) //TODO error page
				return RedirectToAction("ViewClub", "Clubs", new { id });
			return RedirectToAction("Invites");
		}

		[Authorize]
		public async Task<IActionResult> DeclineInvite(int id)
		{
			var file = Request.Form.Files[0];
			if (await memberService.TryDeclineInvite(id, UserId)) //TODO error page
				;
			return RedirectToAction("Invites");
		}
	}
}
