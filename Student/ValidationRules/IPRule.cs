using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Student.ValidationRules
{
    public class IPRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var ip = (string)value;
            if (string.IsNullOrEmpty(ip) || string.IsNullOrWhiteSpace(ip))
                return new ValidationResult(false, "IP cannot be empty");

            var reg = new Regex(@"^(([0-9]{1,3}\.){3}[0-9]{1,3}|localhost)$");
            var match = reg.Match(ip);
            return match.Success
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "IP address invalid");
        }
    }
}