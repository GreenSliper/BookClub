using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookClub.Models
{
	public class BookPickerModel
	{
		public int entityID { get; set; }
		public List<int> ids { get; set; } = new List<int>();
		public List<bool> added { get; set; } = new List<bool>();

		public BookPickerModel()
		{ 
		}
		public BookPickerModel(int entityID, IEnumerable<DAL.Models.Book> books, bool add = false)
		{
			this.entityID = entityID;
			Add(books, add);
		}

		public BookPickerModel(int entityID, ICollection<DAL.Models.ClubBook> books, bool add = false)
		{
			this.entityID = entityID;
			Add(books, add);
		}

		public void Add(IEnumerable<DAL.Models.Book> books, bool add = false)
		{
			foreach (var b in books)
			{
				ids.Add(b.ID.Value);
				added.Add(add);
			}
		}

		public void Add(IEnumerable<DAL.Models.ClubBook> books, bool add = false)
		{
			foreach (var b in books)
			{
				ids.Add(b.Book.ID.Value);
				added.Add(add);
			}
		}
	}
}
