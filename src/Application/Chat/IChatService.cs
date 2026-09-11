using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat
{
    public interface IChatService
    {
        IAsyncEnumerable<string> GetResponseStreamingAsync(ChatRequest request, CancellationToken cancellationToken=default);
    }
}

