using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Chat.Commands.SendMessage
{
    public class SendMessageHandler:IRequestHandler<SendMessageCommand,ChatResponse>
    {
        private readonly IChatService _chatService;
        private readonly IChatSessionService _chatSessionService;


        public SendMessageHandler(IChatService chatService, IChatSessionService chatSessionService)
        {
            _chatService = chatService;
            _chatSessionService = chatSessionService;
        }

        public async Task<ChatResponse> Handle(SendMessageCommand request,CancellationToken cancellationToken)
        {
            var sessionExists = await _chatSessionService.IsSessionExistAsync(request.SessionId);

            if (!sessionExists)
            {
                throw new KeyNotFoundException("Session not found.");
            }

            await _chatSessionService.AddMessageAsync(request.SessionId, ChatRole.User, request.Message);
            var chatRequest = new ChatRequest
            {
                SessionId = request.SessionId,
                Message = request.Message
            };
            var result = await _chatService.GetResponseAsync(chatRequest);
            await _chatSessionService.AddMessageAsync(request.SessionId, ChatRole.Assistant, result.Response);
            return result;
        }


    }
}
