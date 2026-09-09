using Application.Chat;
using Domain.Entities;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Tools;

namespace Infrastructure
{
    public class MafAgentService : IChatService
    {
        private readonly AIAgent _agent;

        public MafAgentService(IConfiguration config,WeatherTool weatherTool,CurrencyTool currencyTool)
        {
            var _apiKey = config["OpenAI:apiKey"];


            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            };

            OpenAIClient client = new OpenAIClient(new ApiKeyCredential(_apiKey), options);
            var chatClient = client.GetChatClient("openai/gpt-4o-mini");


            _agent = chatClient.AsAIAgent(
                instructions: "You are a helpful assistant.",
                name: "ChatAssistant",
                tools: [AIFunctionFactory.Create(weatherTool.GetWeather),
                        AIFunctionFactory.Create(currencyTool.GetExchangeRate)
                ]
            );
        }


        public async IAsyncEnumerable<string> GetResponseStreamingAsync(ChatRequest request,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach(var update in _agent.RunStreamingAsync(request.Message, cancellationToken: cancellationToken))
            {
                if (!string.IsNullOrEmpty(update.Text))
                {
                    yield return update.Text;
                }
            }
        }
    }
}
