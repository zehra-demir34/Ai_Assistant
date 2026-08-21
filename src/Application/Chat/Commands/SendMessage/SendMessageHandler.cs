using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Commands.SendMessage
{
    public class SendMessageHandler:IRequestHandler<SendMessageCommand,ChatResponse>
    {
        private readonly IChatService _chatService;
        private readonly IApplicationDbContext _context;


        public SendMessageHandler(IChatService chatService, IApplicationDbContext context)
        {
            _chatService = chatService;
            _context = context;
        }

        public async Task<ChatResponse> Handle(SendMessageCommand request,CancellationToken cancellationToken)
        {
            var sessionExists = await _context.ChatSessions.AsNoTracking().AnyAsync(x=>x.SessionId==request.SessionId,cancellationToken);

            if (!sessionExists)
            {
                throw new KeyNotFoundException("Session not found.");
            }

            var userMessage = new ChatMessage
            {
                MessageId = Guid.NewGuid(),
                SessionId = request.SessionId,
                Role = ChatRole.User,
                Content = request.Message
            };

            await _context.ChatMessages.AddAsync(userMessage,cancellationToken);

            var chatRequest = new ChatRequest
            {
                SessionId = request.SessionId,
                Message = request.Message
            };

            var result = await _chatService.GetResponseAsync(chatRequest);

            var assistantMessage = new ChatMessage
            {
                MessageId = Guid.NewGuid(),
                SessionId = request.SessionId,
                Role = ChatRole.Assistant,
                Content = result.Response
            };

            await _context.ChatMessages.AddAsync(assistantMessage,cancellationToken);
            await _context.SaveChangesAsync();
            return result;
        }


    }
}
