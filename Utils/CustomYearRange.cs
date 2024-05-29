using System;
using System.ComponentModel.DataAnnotations;
namespace OC_Express_Voitures.Utils;


public class CustomYearRangeAttribute : ValidationAttribute
{
    private int _min;
    private int _max;

    public CustomYearRangeAttribute()
    {
        _min = Constants.OldestYear;
        _max = DateTime.Now.Year;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is int intValue)
        {
            if (intValue < _min || intValue > _max)
            {
                return new ValidationResult($"Value should be between {_min} and {_max}");
            }

            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid type");
    }
}
