using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Queries.GetMessage
{
    public class GetMessageQueryHandler : IRequestHandler< GetMessageQuery, List<ChatMessage>>
    {
        private readonly IApplicationDbContext _context;

        public GetMessageQueryHandler( IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatMessage>> Handle(GetMessageQuery request,CancellationToken cancellationToken)
        {
            var sessionExists = await _context.ChatSessions.AsNoTracking().AnyAsync(x=>x.SessionId==request.SessionId,cancellationToken);

            if (!sessionExists)
            {
                throw new KeyNotFoundException("Session not found.");
            }

            return await _context.ChatMessages.AsNoTracking()
                .Where(x=>x.SessionId==request.SessionId)
                .OrderBy(x=>x.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
