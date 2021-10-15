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
		public virtual ReaderUser AddedByUser { get; set; }
		public virtual ICollection<ClubBook> Clubs { get; set; }
		public virtual ICollection<ReadBook> ReadBy { get; set; }
		public virtual ICollection<ClubDiscussionBook> IncludedInDiscussions { get; set; }
	}
}
