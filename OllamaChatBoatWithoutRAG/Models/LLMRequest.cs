namespace OllamaChatBoatWithoutRAG.Models
{
    public class LLMRequest
    {
        public List<ChatMessage> Messages { get; set; } = new();
        public double Temperature { get; set; } = 0.7;
        public int MaxTokens { get; set; } = 2048;
        public bool Stream { get; set; }

    }
}
