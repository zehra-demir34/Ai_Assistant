using Application.Chat;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace web.api.Controllers
{
    [Route("chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IChatSessionService _chatSessionService;

        public ChatController(IChatService chatService,IChatSessionService chatSessionService)
        {
            _chatService = chatService;
            _chatSessionService = chatSessionService;
        }

        [HttpPost("sessions")]
        public async Task<IActionResult> CreateSession()
        {
            var sessionId = await _chatSessionService.CreateSessionAsync();
            return Ok( sessionId );
        }

        [HttpPost]
        public async Task<IActionResult> Chat(ChatRequest request)
        {
            if (!await _chatSessionService.IsSessionExistAsync(request.SessionId))
            {
                return NotFound("Session not found.");
            }
            await _chatSessionService.AddMessageAsync(request.SessionId, ChatRole.User, request.Message);

            var result=await _chatService.GetResponseAsync(request);

            await _chatSessionService.AddMessageAsync(request.SessionId,ChatRole.Assistant, result.Response);

            return Ok(result);
        }

        [HttpGet("sessions/{sessionId}/messages")]
        public async Task<IActionResult> GetMessages(Guid sessionId)
        {

            if (!await _chatSessionService.IsSessionExistAsync(sessionId))
            {
                return NotFound("Session not found.");
            }
            var messages=await _chatSessionService.GetChatMessageAsync(sessionId);
            return Ok(messages);
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> GetSessions()
        {
            var sessions=await _chatSessionService.GetChatSessionsAsync();
            return Ok(sessions);
        }

        [HttpDelete("sessions/{sessionId}")]
        public async Task<IActionResult> DeleteSession(Guid sessionId)
        {
            if (!await _chatSessionService.IsSessionExistAsync(sessionId))
            {
                return NotFound("Session not found.");
            }

            await _chatSessionService.DeleteSessionAsync(sessionId);
            return NoContent();
        }
    }
}
