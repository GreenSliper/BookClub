using System;
using System.Collections.Generic;
using DAL.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class ClubBook
	{
		public int ClubID { get; set; }
		public int BookID { get; set; }

		public Club Club { get; set; }
		public Book Book { get; set; }
		public DateTime AddedTime { get; set; }
		public ReaderUser AddedByUser { get; set; }
	}
}
