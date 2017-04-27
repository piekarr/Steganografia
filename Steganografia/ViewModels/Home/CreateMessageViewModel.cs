using System.ComponentModel.DataAnnotations;

namespace Steganografia.ViewModels.Home
{
    public class CreateMessageViewModel
    {
        [Required]
        public int ConversatonId { get; set; }

        [Required]
        [StringLength(maximumLength:500)]
        public string Message { get; set; }
    }
}