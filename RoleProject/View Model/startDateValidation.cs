using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoleProject.View_Model
{
    public class startDateValidation: ValidationAttribute
    {

    
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
            DateTime start = Convert.ToDateTime(value);
                if (start >= DateTime.Now)
                    return ValidationResult.Success;
                return new ValidationResult("cannot div by three y assal");
            
        }
    }
}