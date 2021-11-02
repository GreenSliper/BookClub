using DAL.Data;
using DAL.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Club
	{
		public int? ID { get; set; }

		[Display(Name = "Club Name")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "Club name must be assigned")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Club name must be between {2} and {1} characters long")]
		public string Name { get; set; }
		
		[Display(Name = "Description")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "Please, write a few words about your club")]
		[StringLength(255, MinimumLength = 10, ErrorMessage = "Club description must be between {2} and {1} characters long")]
		public string Description { get; set; }
		
		[Display(Name = "Is Public?")]
		public bool IsPublic { get; set; }
		
		[Display(Name = "Avatar")]
		[ImageResolution(1, targetResolutionError:"Image should be square")]
		public Image AvatarImage { get; set; }
		public ReaderUser Creator { get; set; }
		public ICollection<ClubMember> Members { get; set; }
		public ICollection<ClubBook> Books { get; set; }
		public ICollection<ClubDiscussion> Discussions { get; set; }
		public ICollection<ClubDiscussion> ActiveDiscussions { get => Discussions?.Where(x => x.Time >= DateTime.Now).ToArray(); }
		public ICollection<ClubDiscussion> ArchivedDiscussions { get => Discussions?.Where(x => x.Time < DateTime.Now).ToArray(); }
	}
}
