using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Steganografia.ViewModels.Home
{
    public class CreateConversationViewModel
    {
        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 3)]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Users")]
        public List<int> UserIds { get; set; }
    }
}