using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat
{
    public interface IApplicationDbContext
    {
        DbSet<ChatSession> ChatSessions { get; }
        DbSet<ChatMessage> ChatMessages { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
