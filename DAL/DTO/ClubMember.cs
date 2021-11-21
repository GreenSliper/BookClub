using DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public enum MemberPermissions { Reader, Manager, Admin, Creator }
	public class ClubMember
	{
		public int ClubID { get; set; }
		public string UserID { get; set; }

		public virtual ReaderUser User { get; set; }
		public virtual Club Club { get; set; }
		public DateTime LastVisitTime { get; set; }
		public MemberPermissions PermissionLevel { get; set; }
	}
}
