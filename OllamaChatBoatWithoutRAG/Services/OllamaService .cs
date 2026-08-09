using Microsoft.Extensions.Options;
using OllamaChatBoatWithoutRAG.DTOS.ollama;
using OllamaChatBoatWithoutRAG.Enums;
using OllamaChatBoatWithoutRAG.Interfaces;
using OllamaChatBoatWithoutRAG.Models;
using OllamaChatBoatWithoutRAG.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace OllamaChatBoatWithoutRAG.Services
{
    public class OllamaService : ILLMService
    {
        private readonly HttpClient _client;
        private readonly OllamaOptions _options;
        public OllamaService(HttpClient client, IOptions<OllamaOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        private static string GetRole(ChatRole role)
        {
            return role switch
            {
                ChatRole.System => "system",
                ChatRole.User => "user",
                ChatRole.Assistant => "assistant",
                _ => "user"
            };
        }


        public async Task<LLMResponse> ChatAsync(LLMRequest messages)
        {
            var request = new OllamaChatRequest
            {
                Model = _options.Model,
                Stream = false,
                Messages = messages.Messages
                .Select(x =>
                    new OllamaMessage
                    {
                        Role = GetRole(x.Role),
                        Content = x.Content
                    })
                .ToList()
            };


            var response =await _client.PostAsJsonAsync("/api/chat",request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaChatResponse>();

            //    return result?.Message.Content ?? "";

            return new LLMResponse
            {
                Content = result?.Message.Content ?? "",
                Model = _options.Model,
                Success = true
            };

        }

        public async IAsyncEnumerable<string> StreamAsync(LLMRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var ollamaRequest = new OllamaChatRequest
            {
                Model = _options.Model,
                Stream = true,
                Messages = request.Messages
                    .Select(x => new OllamaMessage
                    {
                        Role = GetRole(x.Role),
                        Content = x.Content
                    })
                    .ToList()
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post,"/api/chat");

            httpRequest.Content = JsonContent.Create(ollamaRequest);

            var response = await _client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
           
            response.EnsureSuccessStatusCode();
            
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            
            using var reader = new StreamReader(stream);
            
            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();
            
                var line = await reader.ReadLineAsync();
                
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                
                var chunk = JsonSerializer.Deserialize<OllamaStreamResponse>(line);
                
                if (chunk == null)
                    continue;
                
                if (chunk.Done)
                    yield break;
                
                if (!string.IsNullOrWhiteSpace(chunk.Message.Content))
                    yield return chunk.Message.Content;
            }
        }

    }
}

