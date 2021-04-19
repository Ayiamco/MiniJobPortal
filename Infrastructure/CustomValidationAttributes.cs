using inSpark.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace inSpark.Infrastructure.Services
{
    public class MustBeFutureDate : ValidationAttribute
    {
        //
        //Deadline must  be a future date
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var obj = (JobFormViewModel)validationContext.ObjectInstance;
            if (obj.DeadLine.Year  > DateTime.Now.Year || obj.DeadLine.ToString()== "1/1/0001 12:00:00 AM")
                return ValidationResult.Success;
            else if (obj.DeadLine.Year == DateTime.Today.Year && obj.DeadLine.DayOfYear > DateTime.Now.DayOfYear)
                return ValidationResult.Success;
            else
                return new ValidationResult("Job Deadline Already Past");
            

        }
    }

    
}






