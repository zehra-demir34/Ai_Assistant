using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Queries.GetMessage
{
    public record GetMessageQuery(Guid SessionId) : IRequest<List<ChatMessage>>;
}
