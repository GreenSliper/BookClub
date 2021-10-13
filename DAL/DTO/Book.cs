using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class Book
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		public ReaderUser AddedByUser { get; set; }
		public ICollection<ClubBook> Clubs { get; set; }
		public ICollection<ReadBook> ReadBy { get; set; }
	}
}
