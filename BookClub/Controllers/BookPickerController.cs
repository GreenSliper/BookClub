using BookClub.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookClub.Controllers
{
	public class BookPickerController : Controller
	{
		public static readonly string PickerContainerName = "selectedBooks";

		[HttpPost]
		public void ToggleBook(int id)
		{
			List<int> selectedBooks;
			if (SessionHelper.GetObjectFromJson<List<int>>(HttpContext.Session, PickerContainerName) == null)
				selectedBooks = new List<int>();
			else
				selectedBooks = SessionHelper.GetObjectFromJson<List<int>>(HttpContext.Session, PickerContainerName);
			//toggle
			if(!selectedBooks.Remove(id))
				selectedBooks.Add(id);
			SessionHelper.SetObjectAsJson(HttpContext.Session, PickerContainerName, selectedBooks);
		}
	}
}
