using System.ComponentModel.DataAnnotations;
#nullable disable
namespace WebApplication1_Test.Models
{
    public class SignInModel
    {
        [Display(Name = "EmailOrName")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "StringLengthOverMax")]
        public string EmailOrName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }
}
