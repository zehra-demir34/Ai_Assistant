using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ChatSession
    {
        [Key]
        public Guid SessionId { get; set; }
        public DateTime CreatedAt {  get; set; }= DateTime.UtcNow;
        public ICollection<ChatMessage> Messages { get; set; }= new List<ChatMessage>();
    }
}
