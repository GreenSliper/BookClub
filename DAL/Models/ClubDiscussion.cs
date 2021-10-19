using DAL.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
		[ValidateNever]
		public virtual Club Club { get; set; }
		[ValidateNever]
		public virtual ReaderUser Creator { get; set; }
		[ValidateNever]
		public virtual List<ClubDiscussionBook> Books { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime Time { get; set; } = DateTime.Now;
	}
}
