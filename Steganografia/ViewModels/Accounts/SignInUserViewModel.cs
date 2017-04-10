using System.ComponentModel.DataAnnotations;

namespace Steganografia.ViewModels.Accounts
{
    public class SignInUserViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}