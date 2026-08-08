namespace OllamaChatBoatWithoutRAG.DTOS.ollama
{
    public class OllamaChatRequest
    {
        public string Model { get; set; } = "";
        public List<OllamaMessage> Messages { get; set; }
            = new();
        public bool Stream { get; set; }

    }
}
