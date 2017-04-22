using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steganografia.ViewModels.Home
{
    public class ConversationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UnreadMessages { get; set; }
    }
}