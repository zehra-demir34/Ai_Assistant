using Application.Chat;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ChatSessionService:IChatSessionService
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatSessionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Guid> CreateSessionAsync()
        {
            var session = new ChatSession { SessionId = Guid.NewGuid() };
            _dbContext.ChatSessions.Add(session);
            await _dbContext.SaveChangesAsync();
            return session.SessionId;
        }

        public async Task<bool> IsSessionExistAsync(Guid sessionId)
        {
            return await _dbContext.ChatSessions.AnyAsync(session => session.SessionId == sessionId);
        }

        public async Task AddMessageAsync(Guid sessionId,ChatRole role,string content)
        {
            var message = new ChatMessage
            {
                MessageId = Guid.NewGuid(),
                SessionId = sessionId,
                Role = role,
                Content = content
            };
            _dbContext.ChatMessages.Add(message);
            await _dbContext.SaveChangesAsync();    
        }

        public async Task<List<ChatMessage>> GetChatMessageAsync(Guid sessionId)
        {
            return await _dbContext.ChatMessages.Where(x => x.SessionId == sessionId).ToListAsync();
            
        }

        public async Task<List<ChatSession>> GetChatSessionsAsync()
        {
            return await _dbContext.ChatSessions.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task DeleteSessionAsync(Guid sessionId)
        {
            var deletedSession=await _dbContext.ChatSessions.FirstOrDefaultAsync(x=>x.SessionId==sessionId);
            _dbContext.ChatSessions.Remove(deletedSession);
            await _dbContext.SaveChangesAsync();
        }

    }
}
