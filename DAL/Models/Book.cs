using DAL.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Book
	{
		public int? ID { get; set; }
		public string Name { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		[ValidateNever]
		[Display(Name = "Rating")]
		public float AverageRating { get; set; }
		[ValidateNever]
		[Display(Name = "Reviews")]
		public int RatingCount { get; set; }
	}
}
