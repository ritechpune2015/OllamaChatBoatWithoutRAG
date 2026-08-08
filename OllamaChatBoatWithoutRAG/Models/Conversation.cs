namespace OllamaChatBoatWithoutRAG.Models
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime CreatedOn { get; set; }
        public List<ChatMessage> Messages { get; set; } = new();

    }
}
