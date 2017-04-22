using System;

namespace Steganografia.ViewModels.Home
{
    public class ConversationMessageViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUser { get; set; }
    }
}