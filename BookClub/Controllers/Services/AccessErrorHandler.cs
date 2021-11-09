using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookClub.Controllers.Services
{
	public class AccessErrorHandler<ModelT, ErrorModelT, ErrorT> : 
		ErrorDisplayHandler<ModelAccessResult<ModelT, ErrorModelT, ErrorT>, ErrorT>
		where ErrorT: Enum
		where ModelT: class
		where ErrorModelT: class
	{
		private readonly string errorUndefinedActionName;
		public AccessErrorHandler(Controller controller, string errorUndefinedActionName="Index") : base(controller)
		{
			this.errorUndefinedActionName = errorUndefinedActionName;
		}

		public override IActionResult HandlerNotFoundAction()
		{
			return controller.RedirectToAction(errorUndefinedActionName);
		}

		protected override async Task<IActionResult> ProcessRequest(ModelAccessResult<ModelT, ErrorModelT, ErrorT> request,
			string normalViewName, object replaceModel = null)
		{
			if (request.Success)
				return controller.View(normalViewName, replaceModel??request.requestedModel);
			else
				return await ProcessRequestErrors(request);
		}

		protected override async Task<IActionResult> ProcessRequestErrors(ModelAccessResult<ModelT, ErrorModelT, ErrorT> request)
		{
			return await DisplayError(request.error, request.errorModel) ?? controller.RedirectToAction(errorUndefinedActionName);
		}
	}
}