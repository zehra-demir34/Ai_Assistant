using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Queries.GetSession
{
    public class GetSessionQueryHandler : IRequestHandler<GetSessionQuery,List<ChatSession>>
    {
        private readonly IChatSessionService _chatSessionService;

        public GetSessionQueryHandler(IChatSessionService chatSessionService)
        {
            _chatSessionService = chatSessionService;
        }
        public async Task<List<ChatSession>> Handle(GetSessionQuery request,CancellationToken cancellationToken)
        {
            return await _chatSessionService.GetChatSessionsAsync();
        }
    }
}
