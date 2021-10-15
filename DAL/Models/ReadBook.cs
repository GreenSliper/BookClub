using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ReadBook
	{
		public virtual ReaderUser Reader { get; set; }
		public virtual Book Book { get; set; }

		public int? RememberQuality { get; set; }
		public int? Rating { get; set; }
	}
}
