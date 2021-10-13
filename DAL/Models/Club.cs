using DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Club
	{
		public int ID { get; set; }

		[Display(Name = "Club Name")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "Club name must be assigned")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Club name must be between {2} and {1} characters long")]
		public string Name { get; set; }
		
		[Display(Name = "Description")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "Please, write a few words about your club")]
		[StringLength(255, MinimumLength = 10, ErrorMessage = "Club name must be between {2} and {1} characters long")]
		public string Description { get; set; }
		
		[Display(Name = "Is Public?")]
		public bool IsPublic { get; set; }
		public ReaderUser Creator { get; set; }
		public ICollection<ClubMember> Members { get; set; }
		public ICollection<Book> Books { get; set; }
	}
}
