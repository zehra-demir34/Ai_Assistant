using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Queries.GetMessage
{
    public class GetMessageQueryHandler : IRequestHandler< GetMessageQuery, List<ChatMessage>>
    {
        private readonly IChatSessionService _chatSessionService;

        public GetMessageQueryHandler(IChatSessionService chatSessionService)
        {
            _chatSessionService = chatSessionService;
        }

        public async Task<List<ChatMessage>> Handle(GetMessageQuery request,CancellationToken cancellationToken)
        {
            var sessionExists = await _chatSessionService.IsSessionExistAsync(request.SessionId);

            if (!sessionExists)
            {
                throw new KeyNotFoundException("Session not found.");
            }

            return await _chatSessionService.GetChatMessageAsync(request.SessionId);
        }
    }
}
