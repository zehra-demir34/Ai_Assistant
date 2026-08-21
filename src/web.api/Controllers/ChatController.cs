using Application.Chat;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Chat.Commands.CreateSession;
using Application.Chat.Commands.SendMessage;
using Application.Chat.Queries.GetMessage;
using Application.Chat.Queries.GetSession;
using Application.Chat.Commands.DeleteSession;

namespace web.api.Controllers
{
    [Route("chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sessions")]
        public async Task<IActionResult> CreateSession(CancellationToken ct)
        {
            var sessionId = await _mediator.Send(new CreateSessionCommand(),ct);
            return Ok( sessionId );
        }

        [HttpPost]
        public async Task<IActionResult> Chat(SendMessageCommand command,CancellationToken ct)
        {
            
            var result=await _mediator.Send(command,ct);
            return Ok(result);
        }

        [HttpGet("sessions/{sessionId}/messages")]
        public async Task<IActionResult> GetMessages(Guid sessionId,CancellationToken ct)
        {

            var messages=await _mediator.Send(new GetMessageQuery(sessionId),ct);
            return Ok(messages);
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> GetSessions(CancellationToken ct)
        {
            var sessions=await _mediator.Send(new GetSessionQuery(),ct);
            return Ok(sessions);
        }

        [HttpDelete("sessions/{sessionId}")]
        public async Task<IActionResult> DeleteSession(Guid sessionId,CancellationToken ct)
        {
           
            await _mediator.Send(new DeleteSessionCommand(sessionId),ct);
            return NoContent();
        }
    }
}
