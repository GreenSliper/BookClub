using DAL.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Data
{
	public class ReaderUser : IdentityUser
	{
		public string Status { get; set; }
		public virtual ICollection<ClubMember> Memberships { get; set; }
		public virtual ICollection<ReadBook> ReadBooks { get; set; }
		public virtual ICollection<ClubDiscussion> CreatedDiscussions { get; set; }
	}
}
