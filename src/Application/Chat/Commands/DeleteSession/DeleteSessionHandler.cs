using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Commands.DeleteSession
{
    public class DeleteSessionHandler : IRequestHandler<DeleteSessionCommand>
    {
        private readonly IChatSessionService _chatSessionService;
        public DeleteSessionHandler(IChatSessionService chatSessionService)
        {
            _chatSessionService = chatSessionService;
        }
        public async Task Handle(DeleteSessionCommand request,CancellationToken cancellationToken)
        {
            var sessionExists = await _chatSessionService.IsSessionExistAsync(request.SessionId);
            if (!sessionExists)
            {
                throw new KeyNotFoundException("Session not found.");
            }
            await _chatSessionService.DeleteSessionAsync(request.SessionId);
        }

    }
}
