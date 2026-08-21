using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Chat.Commands.SendMessage
{
    public record SendMessageCommand(Guid SessionId, string Message) : IRequest<ChatResponse>;

}
