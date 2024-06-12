using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Homework.CustomAttributes
{  
    public class StringValueIn : ValidationAttribute
    {
        public string[] Values { get; }
        public StringValueIn(string[] values)
        {
            Values = values;
        }
        public string GetErrorMessage() =>
        $"Value of field must be in {string.Join(", ",Values)}.";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            value = (string)value;
            if (!Values.Contains(value))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
