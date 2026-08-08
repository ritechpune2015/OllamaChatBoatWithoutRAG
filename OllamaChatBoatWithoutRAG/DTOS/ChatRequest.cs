namespace OllamaChatBoatWithoutRAG.DTOS
{
    public class ChatRequest
    {
        public Guid ConversationId { get; set; }
        public string Message { get; set; } = "";

    }
}
