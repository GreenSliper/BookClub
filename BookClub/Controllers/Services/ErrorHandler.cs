using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookClub.Controllers.Services
{
	public interface IRequestErrorHandler
	{
		Task<IActionResult> GetErrorView(object errorModel);
		void SetController(Controller controller);
		/// <summary>
		/// Add task that should be executed before GetErrorView
		/// </summary>
		/// <returns>This object (for fluent)</returns>
		/// <param name="task">Takes the error-model as input</param>
		IRequestErrorHandler AddTask(Func<object, Task> task);
	}
	public abstract class ErrorHandlerBase : IRequestErrorHandler
	{
		protected Controller controller;
		protected Func<object, Task> beforeExecutionTask;
		public IRequestErrorHandler AddTask(Func<object, Task> task)
		{
			beforeExecutionTask += task;
			return this;
		}
		public void SetController(Controller controller) => this.controller = controller;
		public async Task<IActionResult> GetErrorView(object errorModel)
		{
			if (beforeExecutionTask != null)
				await beforeExecutionTask.Invoke(errorModel);
			return GetView(errorModel);
		}
		protected abstract IActionResult GetView(object errorModel);
	}
	public class ErrorHandler : ErrorHandlerBase
	{
		private Func<object, IActionResult> displayErrorView;
		public ErrorHandler(Func<object, IActionResult> displayErrorView)
		{
			this.displayErrorView = displayErrorView;
		}

		public ErrorHandler(string viewName)
		{
			displayErrorView = obj=>controller.View(viewName, obj);
		}
		protected override IActionResult GetView(object errorModel) => displayErrorView?.Invoke(errorModel);
	}
}
