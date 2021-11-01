using DAL.Models.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Image
	{
		public string URL { get; set; }
		[MaxFileSize(2 * 1024 * 1024)]
		[AllowedExtensions(".jpg", ".png")]
		public IFormFile File { get; set; }
	}
}
