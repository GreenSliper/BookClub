using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ClubDiscussionBook
	{
		public virtual ClubDiscussion Discussion { get; set; }
		public virtual Book Book { get; set; }
		[Range(1, 5)]
		public int Priority { get; set; }
	}
}
