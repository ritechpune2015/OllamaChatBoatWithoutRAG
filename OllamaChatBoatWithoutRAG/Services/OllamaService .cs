using Microsoft.Extensions.Options;
using OllamaChatBoatWithoutRAG.DTOS.ollama;
using OllamaChatBoatWithoutRAG.Enums;
using OllamaChatBoatWithoutRAG.Interfaces;
using OllamaChatBoatWithoutRAG.Models;
using OllamaChatBoatWithoutRAG.Options;

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


        public async Task<string> ChatAsync(List<ChatMessage> messages)
        {
            var request = new OllamaChatRequest
            {
                Model = _options.Model,
                Stream = false,
                Messages = messages
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

            return result?.Message.Content ?? "";
        }
    }
}
