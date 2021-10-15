using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class ClubDiscussionBook
	{
		public int DiscussionID { get; set; }
		public int BookID { get; set; }
		public virtual ClubDiscussion Discussion { get; set; }
		public virtual Book Book { get; set; }
		public int Priority { get; set; }
	}
}
