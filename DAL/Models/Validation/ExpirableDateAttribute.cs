using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Validation
{
	public class ExpirableDateAttribute : ValidationAttribute
	{
		private readonly TimeSpan maxExpirationTime;
		private readonly TimeSpan? minExpirationTime;
		public ExpirableDateAttribute(string maxExpirationTimeSpan, string minExpirationTimeSpan = null)
		{
			if (!TimeSpan.TryParse(maxExpirationTimeSpan, out maxExpirationTime))
				throw new ArgumentException("maxExpirationTimeSpan should be a valid TimeSpan string");
			if (minExpirationTimeSpan != null)
			{
				if (TimeSpan.TryParse(minExpirationTimeSpan, out TimeSpan minExp))
					minExpirationTime = minExp;
				else
					throw new ArgumentException("minExpirationTimeSpan should be a valid TimeSpan string");
			}
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (validationContext.ObjectInstance is not IModelExpirable model)
			{
				throw new ValidationException("Model using ExpirableDateAttribute should implement IModelExpirable interface");
			}
			if (model.ExpirationTime <= DateTime.Now)
				return new ValidationResult("Expiration time should be later than current");
			if (minExpirationTime != null)
			{
				var minTime = DateTime.Now.Add(minExpirationTime.Value);
				if (model.ExpirationTime <= minTime)
					return new ValidationResult($"Expiration time should be at least {minTime:g}");
			}
			var maxTime = DateTime.Now.Add(maxExpirationTime);
			if (model.ExpirationTime > maxTime)
				return new ValidationResult($"Max expiration time is {maxTime:g}");
			return ValidationResult.Success;
		}
	}
}
