using System.ComponentModel.DataAnnotations;

namespace Steganografia.ViewModels.Accounts
{
    public class RegisterUserViewModel : SignInUserViewModel
    {
        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat Password")]
        [StringLength(50, MinimumLength = 6)]
        public string RepeatPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; }
    }
}