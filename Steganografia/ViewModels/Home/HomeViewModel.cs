using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steganografia.ViewModels.Home
{
    public class HomeViewModel
    {
        public IEnumerable<ConversationViewModel> Conversations { get; set; }
    }
}