using System.Globalization;
using System.Net;
using System.Windows.Controls;

namespace Student.ValidationRules
{
    public class PortRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var portString = (string)value;
            if (string.IsNullOrEmpty(portString) || string.IsNullOrWhiteSpace(portString))
                return new ValidationResult(false, "Port Cannot be empty");
            if (!int.TryParse(portString, out var port))
                return new ValidationResult(false, "Invalid Port number");
            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
                return new ValidationResult(false, $"Port outside range of {IPEndPoint.MinPort}--{IPEndPoint.MaxPort}");
            return ValidationResult.ValidResult;
        }
    }
}