using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Student.ValidationRules
{
    public class FullNameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var fullName = (string)value;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrWhiteSpace(fullName))
                return new ValidationResult(false, "Full Name cannot be empty!");

            var regex = new Regex(@"^[A-z]+\s[A-z]+\s*[A-z]*$");

            return regex.Match(fullName).Success
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Full Name Invalid");
        }
    }
}