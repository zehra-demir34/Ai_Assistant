using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Document
    {
        public Guid DocumentId { get; set; }
        public Guid UserId {  get; set; }
        public Guid SessionId { get; set; }
        public string FileName {  get; set; }=string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public ChatSession Session { get; set; } = null!;
    }
}
