using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat
{
    public interface IChatSessionService
    {
        Task<Guid> CreateSessionAsync();
        Task<bool> IsSessionExistAsync(Guid sessionId);

        Task AddMessageAsync(Guid sessionId,ChatRole role,string content);

        Task<List<ChatMessage>> GetChatMessageAsync(Guid sessionId);
        Task<List<ChatSession>> GetChatSessionsAsync();
        Task DeleteSessionAsync(Guid sessionId);

    }
}
