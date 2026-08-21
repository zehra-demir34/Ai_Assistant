using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Commands.DeleteSession
{
    public class DeleteSessionHandler : IRequestHandler<DeleteSessionCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeleteSessionHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteSessionCommand request,CancellationToken cancellationToken)
        {

            var session = await _context.ChatSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SessionId == request.SessionId, cancellationToken);

            if (session is null)
            {
                throw new KeyNotFoundException("Session not found.");
            }

            _context.ChatSessions.Remove(session);
            await _context.SaveChangesAsync(cancellationToken);

        }

    }
}
