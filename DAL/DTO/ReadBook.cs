using System;
using System.Collections.Generic;
using DAL.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class ReadBook
	{
		public string ReaderID { get; set; }
		public int BookID { get; set; }

		public virtual ReaderUser Reader { get; set; }
		public virtual Book Book { get; set; }

		public int? RememberQuality { get; set; }
		public int? Rating { get; set; }
	}
}
