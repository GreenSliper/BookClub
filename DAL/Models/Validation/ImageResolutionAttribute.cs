using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Validation
{
	public class ImageResolutionAttribute : ValidationAttribute
	{
		private readonly float widthToHeight, maxWHDelta;
		private readonly int? minWidth, maxWidth;
		private readonly string targetResolutionError;

		/// <summary>
		/// Validate IFormFile as an image
		/// </summary>
		/// <param name="widthToHeight">target resolution</param>
		/// <param name="maxWHDelta">max offset from target resolution (0.01 = +-10%)</param>
		/// <param name="minWidth">Min width in pixels</param>
		/// <param name="maxWidth">Max width in pixels</param>
		/// <param name="targetResolutionError">Error message</param>
		public ImageResolutionAttribute(float widthToHeight, float maxWHDelta = 0.01f, 
			int minWidth = -1, int maxWidth = -1, string targetResolutionError = null)
		{
			this.widthToHeight = widthToHeight;
			this.maxWHDelta = maxWHDelta;
			this.minWidth = minWidth < 0 ? null : minWidth;
			this.maxWidth = maxWidth < 0 ? null : minWidth;
			this.targetResolutionError = targetResolutionError;
		}
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var imgFile = value as Image;
			if (imgFile == null)
				return new ValidationResult("ImageResolutionAttribute should be applied to Image field");
			try
			{
				System.Drawing.Image img = System.Drawing.Image.FromStream(imgFile.File.OpenReadStream());
				if (Math.Abs(img.Width / (float)img.Height - widthToHeight) > maxWHDelta)
					if (targetResolutionError == null)
						return new ValidationResult("Image has incorrect resolution");
					else
						return new ValidationResult(targetResolutionError);
				if(minWidth.HasValue && img.Width < minWidth.Value)
					return new ValidationResult(
						$"Image is too small. Minimum size is {minWidth}x{Math.Round(minWidth.Value/widthToHeight)}");
				else if(maxWidth.HasValue && img.Width > maxWidth.Value)
					return new ValidationResult(
						$"Image is too big. Maximum size is {maxWidth}x{Math.Round(maxWidth.Value / widthToHeight)}");
			}
			catch
			{
				return new ValidationResult("Selected file is not a valid image!");
			}
			return ValidationResult.Success;
		}
	}
}
