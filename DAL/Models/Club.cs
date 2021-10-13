using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Club
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsPublic { get; set; }
		public ReaderUser Creator { get; set; }


		public ICollection<Book> Books { get; set; }
	}
}
