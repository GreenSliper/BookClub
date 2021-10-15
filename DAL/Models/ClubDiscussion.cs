using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ClubDiscussion
	{
		public int ID { get; set; }
		public virtual Club Club { get; set; }
		public virtual ReaderUser Creator { get; set; }
		public virtual ICollection<ClubDiscussionBook> Books { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime Time { get; set; } = DateTime.Now;
	}
}
