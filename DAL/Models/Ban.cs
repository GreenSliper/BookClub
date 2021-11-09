using DAL.Data;
using DAL.Models.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Ban : IModelExpirable
	{
		public int ClubID { get; set; }
		public string BannedUserID { get; set; }
		[ValidateNever]
		public virtual Club Club { get; set; }


		[Display(Name = "Ban expiration time")]
		[ExpirableDate(minExpirationTimeSpan:"01:00")]
		public DateTime ExpirationTime { get; set; } = DateTime.Now.AddDays(1);
		[Display(Name = "Message")]
		public string Message { get; set; }
		[Display(Name = "Forever?")]
		public bool Forever { get; set; }

		public bool IsExpired => DateTime.Now > ExpirationTime;

	}
}
