using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password): IRequest<LoginResponse>;
    public record LoginResponse(string AccessToken, Guid UserId, string Email);
}
