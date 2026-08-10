using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat
{
    public record ChatResponse
    {
        public string Response { get; init; } = string.Empty;
    }
}

