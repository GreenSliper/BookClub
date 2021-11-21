using DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ReadBook
	{
		public int BookID { get; set; }
		public string BookName { get; set; }

		[Display(Name = "How well do you remember the book?")]	
		[Range(1, 10)]
		public int? RememberQuality { get; set; }
		
		[Display(Name = "Rate the book (optional)")]
		[Range(1, 10)]
		public int? Rating { get; set; }
	}
}
