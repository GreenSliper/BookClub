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

		public virtual Club Club { get; set; }
		public virtual Book Book { get; set; }
		public DateTime AddedTime { get; set; }
		public virtual ReaderUser AddedByUser { get; set; }
	}
}
