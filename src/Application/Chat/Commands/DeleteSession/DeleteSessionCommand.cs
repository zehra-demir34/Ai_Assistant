using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Commands.DeleteSession
{
    public record DeleteSessionCommand(Guid SessionId) : IRequest;
}
