using DAL.Data;
using DAL.DTO;
using DAL.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ClubInvite : IModelExpirable
	{

		[Required]
		[Display(Name = "Receiver")]
		public string ReceiverName { get; set; }
		public int ClubID { get; set; }
		public virtual Club Club { get; set; }
		public virtual ReaderUser Inviter { get; set; }

		[Display(Name = "Invite expiration time")]
		[ExpirableDate("30:00:00", "00:30")]
		public DateTime ExpirationTime { get; set; } = DateTime.Now.AddDays(1);
		
		[Display(Name = "Given permissions")]
		public MemberPermissions GivenPermissions { get; set; }

		[Display(Name = "Message")]
		public string Message { get; set; }
	}
}
