using DAL.Data;
using DAL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ClubMember
	{
		[ValidateNever]
		public ReaderUser User { get; set; }
		[ValidateNever]
		public Club Club { get; set; }
		public DateTime LastVisitTime { get; set; }

		[Display(Name = "Role")]
		public MemberPermissions PermissionLevel { get; set; }
	}
}
