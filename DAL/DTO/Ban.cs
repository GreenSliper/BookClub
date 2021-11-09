using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class Ban : IExpirable
	{
		public int ClubID { get; set; }
		public string BannedUserID { get; set; }
		public string BannedByID { get; set; }
		public virtual Club Club { get; set; }
		public virtual ReaderUser BannedUser { get; set; }
		public virtual ReaderUser BannedBy { get; set; }
		public DateTime ExpirationTime { get; set; } = DateTime.MaxValue;
		public string Message { get; set; }
		public bool IsExpired => DateTime.Now > ExpirationTime;
		public bool IsTemporary => ExpirationTime != DateTime.MaxValue;
	}
}
