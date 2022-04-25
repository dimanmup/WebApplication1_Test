using WebApplication1_Test.Attributes;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace WebApplication1_Test.Models
{
    public class SignUpModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "EmailIncorrect")]
        [MaxLength(100, ErrorMessage = "StringLengthOverMax")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "StringLengthIncorrect")]
        [StrongPassword]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "PasswordNotConfirmed")]
        [Display(Name = "PasswordConfirmed")]
        public string PasswordConfirmed { get; set; }
    }
}
