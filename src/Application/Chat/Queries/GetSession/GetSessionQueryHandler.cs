using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Queries.GetSession
{
    public class GetSessionQueryHandler : IRequestHandler<GetSessionQuery,List<ChatSession>>
    {
        private readonly IApplicationDbContext _context;

        public GetSessionQueryHandler( IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ChatSession>> Handle(GetSessionQuery request,CancellationToken cancellationToken)
        {
            return await _context.ChatSessions.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync(cancellationToken);
        }
    }
}
