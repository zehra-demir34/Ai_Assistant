using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Commands.CreateSession
{
    public record CreateSessionCommand:IRequest<Guid>;
    
}
