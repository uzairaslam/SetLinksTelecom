using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.GeneralFolder
{
    //public class CustomValidations
    //{
    //}

    public class CantSameValue : ValidationAttribute
    {
        public CantSameValue(string otherProperty)
        {
            OtherProperty = otherProperty;
        }
        public string OtherProperty { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherValue = validationContext.ObjectType.GetProperty(OtherProperty)
                .GetValue(validationContext.ObjectInstance, null);

            if (value == null || otherValue == null) return ValidationResult.Success;
            if (Convert.ToInt32(otherValue) == Convert.ToInt32(value.ToString())) return new ValidationResult("Both Lines Can't be same");
            return ValidationResult.Success;
            //return base.IsValid(value, validationContext);
        }
    }
}