using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookClub.Controllers.Services
{
	public class ErrorHandlerRedirect : ErrorHandlerBase
	{
		private Func<IActionResult> displayErrorView;
		private string[] preserveRouteValues;

		public ErrorHandlerRedirect(string actionName, string controllerName, params string[] preserveRouteValues)
		{
			this.preserveRouteValues = preserveRouteValues;
			displayErrorView = ()=>
			{
				var routeData = controller.RouteData.Values;
				if (preserveRouteValues != null)
				{
					routeData = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
					foreach (var entry in controller.RouteData.Values)
						if (preserveRouteValues.Contains(entry.Key))
							routeData.Add(entry.Key, entry.Value);
				}
				return controller.RedirectToAction(actionName, controllerName, routeData);
			};
		}

		protected override IActionResult GetView(object errorModel) => displayErrorView?.Invoke();
	}
}
