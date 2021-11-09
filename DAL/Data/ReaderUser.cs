using DAL.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
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
		
		[InverseProperty("Receiver")]
		public virtual ICollection<ClubInvite> ReceivedInvites { get; set; }
		
		[InverseProperty("Inviter")]
		public virtual ICollection<ClubInvite> SentInvites { get; set; }
		[InverseProperty("BannedUser")]
		public virtual ICollection<Ban> ReceivedBans { get; set; }
		[InverseProperty("BannedBy")]
		public virtual ICollection<Ban> GivenBans { get; set; }
	}
}
