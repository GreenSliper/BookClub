using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookClub.Controllers.Services
{
	public interface IErrorDisplayHandler<RequestT, ErrEnumT> where RequestT : IRequestResult
	{
		void SetErrorHandlers(Dictionary<ErrEnumT, IRequestErrorHandler> handlers);
		void SetHandler(ErrEnumT error, IRequestErrorHandler handler);
		//Add handler for one request
		IErrorDisplayHandler<RequestT, ErrEnumT> AddTempHandler(ErrEnumT error, IRequestErrorHandler handler);
		Task<IActionResult> GetResult(RequestT request, string normalViewName, object replaceModel = null);
		Task<IActionResult> GetErrorResult(RequestT request);
	}

	public abstract class ErrorDisplayHandler<RequestT, ErrEnumT> : IErrorDisplayHandler<RequestT, ErrEnumT>
		where ErrEnumT : Enum
		 where RequestT : IRequestResult
	{
		protected readonly Controller controller;
		private Dictionary<ErrEnumT, IRequestErrorHandler> errorHandlers = new Dictionary<ErrEnumT, IRequestErrorHandler>();
		private Dictionary<ErrEnumT, IRequestErrorHandler> tempErrorHandlers = new Dictionary<ErrEnumT, IRequestErrorHandler>();
		public ErrorDisplayHandler(Controller controller)
		{
			this.controller = controller;
		}
		public void SetErrorHandlers(Dictionary<ErrEnumT, IRequestErrorHandler> handlers)
		{
			errorHandlers = new Dictionary<ErrEnumT, IRequestErrorHandler>(handlers);
			foreach (var h in errorHandlers.Values)
				h.SetController(controller);
		}
		public void SetHandler(ErrEnumT error, IRequestErrorHandler handler)
		{
			handler.SetController(controller);
			errorHandlers[error] = handler;
		}
		public IErrorDisplayHandler<RequestT, ErrEnumT> AddTempHandler(ErrEnumT error, IRequestErrorHandler handler)
		{
			handler.SetController(controller);
			tempErrorHandlers[error] = handler;
			return this;
		}
		protected async Task<IActionResult> DisplayError(ErrEnumT error, object errModel)
		{
			if (tempErrorHandlers.TryGetValue(error, out var tempHandler))
				return await tempHandler.GetErrorView(errModel);
			if (errorHandlers.TryGetValue(error, out var handler))
				return await handler.GetErrorView(errModel);
			return HandlerNotFoundAction();
		}
		public async Task<IActionResult> GetResult(RequestT request, string normalViewName, object replaceModel = null)
		{
			var result = await ProcessRequest(request, normalViewName, replaceModel);
			tempErrorHandlers.Clear();
			return result;
		}
		public async Task<IActionResult> GetErrorResult(RequestT request)
		{
			if (request.Success)
				throw new Exception("GetErrorResult should not be called on a successful result");
			return await ProcessRequestErrors(request);
		}
		protected abstract Task<IActionResult> ProcessRequest(RequestT request, string normalViewName, object replaceModel = null);
		protected abstract Task<IActionResult> ProcessRequestErrors(RequestT request);
		public abstract IActionResult HandlerNotFoundAction();

	}
}
