using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Chat.Commands.CreateSession
{
    public class CreateSessionHandler : IRequestHandler<CreateSessionCommand,Guid>
    {
        private readonly IChatSessionService _chatSessionService;
        public CreateSessionHandler(IChatSessionService chatSessionService)
        {
            _chatSessionService = chatSessionService;
        }
        public async Task<Guid> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            return await _chatSessionService.CreateSessionAsync();
        }

       
    }
}
