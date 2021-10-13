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
		public ICollection<ClubMember> Memberships { get; set; }
		public ICollection<ReadBook> ReadBooks { get; set; }
	}
}
