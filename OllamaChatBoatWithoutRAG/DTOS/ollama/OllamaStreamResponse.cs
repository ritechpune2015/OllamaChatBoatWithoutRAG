using System.Text.Json.Serialization;

namespace OllamaChatBoatWithoutRAG.DTOS.ollama
{
    public class OllamaStreamResponse
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "";
        [JsonPropertyName("message")]
        public OllamaMessage Message { get; set; } = new();
        [JsonPropertyName("done")]
        public bool Done { get; set; }

    }
}
