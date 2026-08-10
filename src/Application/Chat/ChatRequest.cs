using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat
{
    public record ChatRequest
    {
        public string Message { get; init; } = string.Empty;

    }
}

