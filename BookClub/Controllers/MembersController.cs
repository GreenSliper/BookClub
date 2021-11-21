using BookClub.Controllers.Services;
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

		private readonly IErrorDisplayHandler<ModelAccessResult<ClubMember, Ban, AccessErrors>, AccessErrors> viewDisplayer;
		private readonly IErrorDisplayHandler<ModelAccessResult<ClubMember, Ban, AccessErrors>, AccessErrors> manageDisplayer;
		private readonly IErrorDisplayHandler<ModelAccessResult<Club, Ban, AccessErrors>, AccessErrors> manageClubDisplayer;

		public MembersController(IClubService clubService, IClubMemberService memberService)
		{
			this.clubService = clubService;
			this.memberService = memberService;

			//configure service request error actions
			viewDisplayer = new AccessErrorHandler<ClubMember, Ban, AccessErrors>(this);
			manageDisplayer = new AccessErrorHandler<ClubMember, Ban, AccessErrors>(this);
			manageClubDisplayer = new AccessErrorHandler<Club, Ban, AccessErrors>(this);

			var commonHandlers = new Dictionary<AccessErrors, IRequestErrorHandler>()
			{
				{ AccessErrors.NoAccess, new ErrorHandlerRedirect("Index", "Clubs") },
				{ AccessErrors.NotFound, new ErrorHandlerRedirect("Index", "Clubs") },
				{ AccessErrors.Banned, new ErrorHandler("~/Views/Clubs/Banned.cshtml") }
			};
			viewDisplayer.SetErrorHandlers(commonHandlers);
			manageDisplayer.SetErrorHandlers(commonHandlers);
			manageClubDisplayer.SetErrorHandlers(commonHandlers);
			manageDisplayer.SetHandler(AccessErrors.NotPermitted,
				new ErrorHandlerRedirect("ViewClub", "Clubs", "id"));
			manageClubDisplayer.SetHandler(AccessErrors.NotPermitted,
				new ErrorHandlerRedirect("ViewClub", "Clubs", "id"));
		}
		string UserId { get => User.FindFirstValue(ClaimTypes.NameIdentifier); }

		[Authorize]
		public async Task<IActionResult> Index(int id)
		{
			var request = await clubService.GetClub(id, UserId, MemberActions.View);
			return await manageClubDisplayer.GetResult(request, View().ViewName);
		}

		[Authorize]
		public async Task<IActionResult> Edit(int id, string userId)
		{
			var request = await memberService.ManageMember(UserId, userId, id);
			return await manageDisplayer.GetResult(request, View().ViewName);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromForm] ClubMember member)
		{
			var request = await memberService.ManageMember(UserId, member.User.Id, member.Club.ID.Value);
			if(!request.Success)
				return await manageDisplayer.GetErrorResult(request);

			if (await memberService.TryUpdateMember(member, ModelState, UserId))
				return RedirectToAction("Index", new { id=member.Club.ID });
			//TODO not the best solution ever (fix lost related entities)
			member.Club = request.requestedModel.Club;
			member.User = request.requestedModel.User;
			return View(member);
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
			var request = await clubService.GetClub(id, UserId, MemberActions.ManageClub);
			if (request.Success)
			{
				return View(new ClubInvite()
				{
					ClubID = request.requestedModel.ID.Value,
					Club = request.requestedModel
				});
			}
			return await manageClubDisplayer.GetErrorResult(request);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SendInvite([FromForm] ClubInvite invite)
		{
			var request = await clubService.GetClub(invite.ClubID, UserId, MemberActions.ManageClub);
			if (request.Success)
			{
				if (await memberService.TrySendInvite(invite, ModelState, UserId))
				{
					//TODO add success page
					return RedirectToAction("ViewClub", "Clubs", new { id = invite.ClubID });
				}
				invite.Club = (await clubService.GetClub(invite.ClubID, UserId, MemberActions.View)).requestedModel;
				return View(invite);
			}
			return await manageClubDisplayer.GetErrorResult(request);
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
			if (await memberService.TryDeclineInvite(id, UserId)) //TODO error page
				;
			return RedirectToAction("Invites");
		}

		[Authorize]
		public async Task<IActionResult> Ban(int id, string userId)
		{
			var request = await memberService.ManageMember(UserId, userId, id);
			if(!request.Success)
				return await manageDisplayer.GetErrorResult(request);
			ViewBag.UserName = request.requestedModel.User.UserName;
			return View(new Ban() { 
				BannedUserID = request.requestedModel.User.Id,
				ClubID = request.requestedModel.Club.ID.Value
			});
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Ban([FromForm] Ban ban)
		{
			var request = await memberService.ManageMember(UserId, ban.BannedUserID, ban.ClubID);
			if (!request.Success)
				return await manageDisplayer.GetErrorResult(request);
			if ((await memberService.Ban(ban, ModelState, UserId)))
				return RedirectToAction("Index", new { id = ban.ClubID });
			else
			{
				ViewBag.UserName = request.requestedModel.User.UserName;
				return View(ban);
			}
		}
	}
}
