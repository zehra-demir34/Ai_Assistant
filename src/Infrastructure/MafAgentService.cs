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

            var prompt = request.Message;

            if (!string.IsNullOrWhiteSpace(request.DocumentContent))
            {
                prompt = $"""
                Answer the user's question using the document content below when it is relevant.

                If the question is related to the document, use the document as context
                and answer in your own words. Do not copy the document word-for-word.

                If the question is not related to the document, answer it normally.

                If the question is related to the document but the answer cannot be found
                in the document, say that the document does not contain enough information.

                --- DOCUMENT ---
                {request.DocumentContent}
                --- END DOCUMENT ---

                 User's question:
                {request.Message}
                """;
            }
            await foreach(var update in _agent.RunStreamingAsync(prompt, cancellationToken: cancellationToken))
            {
                if (!string.IsNullOrEmpty(update.Text))
                {
                    yield return update.Text;
                }
            }
        }
    }
}
