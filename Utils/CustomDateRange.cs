using System.ComponentModel.DataAnnotations;

namespace OC_Express_Voitures.Utils
{
    public class CustomDateRangeAttribute : ValidationAttribute
    {
        private DateOnly _minDate;
        private DateOnly _maxDate;
        int minYear = Constants.OldestYear;
        string minYearString = Constants.OldestYear.ToString();
        string ErrorMessage = "$Date must be between January 1, {minYearString}, and today.";

        public CustomDateRangeAttribute()
        {
            // _minDate = DateOnly.Parse(minDate);

            _minDate = new DateOnly(Constants.OldestYear, 1, 1);

            _maxDate = DateOnly.FromDateTime(DateTime.Today) ;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateOnly dateValue)
            {
                if (dateValue < _minDate || dateValue > _maxDate)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
