using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class ClubInvite : IExpirable
	{
		public int ClubID { get; set; }
		public string ReceiverID { get; set; }
		public string InviterID { get; set; }
		public virtual Club Club { get; set; }
		public virtual ReaderUser Receiver { get; set; }
		public virtual ReaderUser Inviter { get; set; }
		public DateTime ExpirationTime { get; set; }
		public MemberPermissions GivenPermissions { get; set; }
		public string Message { get; set; }

		public bool IsExpired
		{
			get => DateTime.Now > ExpirationTime;
		}
	}
}
