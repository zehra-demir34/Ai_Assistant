using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Chat.Commands.CreateSession
{
    public class CreateSessionHandler : IRequestHandler<CreateSessionCommand,Guid>
    {
        private readonly IApplicationDbContext _context;
        public CreateSessionHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            var session = new ChatSession
            {
                SessionId = Guid.NewGuid(),
            };
            _context.ChatSessions.Add(session);
            await _context.SaveChangesAsync(cancellationToken);
            return session.SessionId;
        }

       
    }
}
