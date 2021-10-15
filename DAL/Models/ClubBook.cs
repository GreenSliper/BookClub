using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ClubBook
	{
		public virtual Club Club { get; set; }
		public virtual Book Book { get; set; }
		public DateTime AddedTime { get; set; }
		public DateTime? ReadUntil { get; set; }
		public virtual ReaderUser AddedByUser { get; set; }
	}
}
