using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ChatMessage
    {
        [Key]
        public Guid MessageId { get; set; }
        public Guid SessionId { get; set; }
        public ChatRole Role { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt {  get; set; }= DateTime.UtcNow;
        public ChatSession Session { get; set; } = null!;
    }
}
