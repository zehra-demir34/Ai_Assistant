using Application.Chat;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MafAgentService : IChatService
    {
        private readonly AIAgent _agent;
        public MafAgentService(IConfiguration config)
        {
            var _apiKey = config["OpenAI:apiKey"];

            /*OpenAIClient client = new OpenAIClient(_apiKey);
            var chatClient = client.GetChatClient("gpt-4o-mini");*/

            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            };

            OpenAIClient client = new OpenAIClient(new ApiKeyCredential(_apiKey), options);
            var chatClient = client.GetChatClient("openai/gpt-4o-mini");


            _agent = chatClient.AsAIAgent(
                instructions: "You are a helpful assistant.",
                name: "ChatAssistant");
        }

        public async Task<ChatResponse> GetResponseAsync(ChatRequest request)
        {
            var response = await _agent.RunAsync(request.Message);
            return new ChatResponse { Response = response.Text };
        }
    }
}
