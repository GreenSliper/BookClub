using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Validation
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] extensions;
        public AllowedExtensionsAttribute(params string[] extensions)
        {
            this.extensions = extensions.Select(x=>x.ToLower()).ToArray();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!extensions.Contains(extension))
                    return new ValidationResult(GetErrorMessage(extension));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string extension)
        {
            return $"{extension} extension is not allowed! Allowed: {string.Join(", ", extensions)}";
        }
    }
}
