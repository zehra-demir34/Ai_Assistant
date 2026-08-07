using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ChatMessage
    {
        public ChatRole Role { get; set; }
        public string content { get; set; } = string.Empty;
    }
}
