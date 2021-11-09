using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class Club
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsPublic { get; set; }
		public virtual ReaderUser Creator { get; set; }
		public virtual DBImage AvatarImage { get; set; }
		public virtual ICollection<ClubMember> Members { get; set; }
		public virtual ICollection<ClubInvite> Invites { get; set; }
		public virtual ICollection<ClubBook> Books { get; set; }
		public virtual ICollection<ClubDiscussion> Discussions { get; set; }
		public virtual ICollection<Ban> BanList { get; set; }
	}
}
