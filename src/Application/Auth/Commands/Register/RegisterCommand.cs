using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Auth.Commands.Register
{
    public record RegisterCommand(string Email, string Password):IRequest<RegisterResponse>;

    public record RegisterResponse(string Message, Guid UserId);
}
