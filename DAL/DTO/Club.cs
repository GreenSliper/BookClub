using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class Club
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsPublic { get; set; }
		public ReaderUser Creator { get; set; }
		public ICollection<ClubMember> Members { get; set; }
		public ICollection<ClubBook> Books { get; set; }
	}
}
