using DAL.Data;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ClubMember
	{
		public ReaderUser User { get; set; }
		public Club Club { get; set; }
		public DateTime LastVisitTime { get; set; }
		public MemberPermissions PermissionLevel { get; set; }
	}
}
