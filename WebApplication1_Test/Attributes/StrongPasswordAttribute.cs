using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;

namespace WebApplication1_Test.Attributes
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        private ResourceManager rm = new ResourceManager("WebApplication1_Test.Resources.SharedResource", typeof(SharedResource).Assembly);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null 
                || !Regex.IsMatch(value.ToString(), "[0-9]+")
                || !Regex.IsMatch(value.ToString(), "[A-Za-z]+")
                || !Regex.IsMatch(value.ToString(), "[^0-9A-Za-z]+"))
            {
                string format = rm.GetString("MustBeStrongPassword", Thread.CurrentThread.CurrentCulture);
                string arg0 = validationContext.DisplayName;
                string message = string.Format(format, arg0);

                return new ValidationResult(message);
            }

            return null;
        }
    }
}
