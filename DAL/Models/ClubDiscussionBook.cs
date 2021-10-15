using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ClubDiscussionBook
	{
		public virtual ClubDiscussion Discussion { get; set; }
		public virtual Book Book { get; set; }
		public int Priority { get; set; }
	}
}
