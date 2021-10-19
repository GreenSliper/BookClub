using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookClub.Controllers
{
	public class FunctionController : Controller
	{
        [HttpPost]
        public ActionResult ViewDataToggleIDList(int targId, string listName)
        {
            if (TempData[listName] == null)
            {
				var list = new List<int>
				{
					targId
				};
				TempData[listName] = list;
            }
            else
            {
                var list = (TempData[listName] as int[]).ToList();
                if (list != null)
                {
                    if (list.Contains(targId))
                        list.Remove(targId);
                    else
                        list.Add(targId);
                    TempData[listName] = list;
                }
            }
            return new EmptyResult();
        }
    }
}
