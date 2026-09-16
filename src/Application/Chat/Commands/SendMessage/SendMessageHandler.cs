using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Commands.SendMessage
{
    public class SendMessageHandler:IRequestHandler<SendMessageCommand,IAsyncEnumerable<string>>
    {
        private readonly IChatService _chatService;
        private readonly IApplicationDbContext _context;


        public SendMessageHandler(IChatService chatService, IApplicationDbContext context)
        {
            _chatService = chatService;
            _context = context;
        }

        public async Task<IAsyncEnumerable<string>> Handle(SendMessageCommand request,CancellationToken cancellationToken)
        {
            var sessionExists = await _context.ChatSessions.AsNoTracking()
                .AnyAsync(x=>x.SessionId==request.SessionId && x.UserId==request.UserId,cancellationToken);

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

         
            return StreamResponse(chatRequest, cancellationToken);
        }

        private async IAsyncEnumerable<string> StreamResponse(ChatRequest chatRequest, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var fullResponse = new StringBuilder();
            var buffer = new StringBuilder();

            await foreach (var chunk in _chatService.GetResponseStreamingAsync(chatRequest, cancellationToken))
            {
                fullResponse.Append(chunk);
                buffer.Append(chunk);

                if(buffer.Length >= 150)
                {
                    yield return buffer.ToString();
                    buffer.Clear();
                }
            }

            if (buffer.Length > 0) {
                yield return buffer.ToString();
            }
        

            var assistantMessage = new ChatMessage
            {
                MessageId = Guid.NewGuid(),
                SessionId = chatRequest.SessionId,
                Role = ChatRole.Assistant,
                Content = fullResponse.ToString()
            };

            await _context.ChatMessages.AddAsync(assistantMessage, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }

      
    }
}
