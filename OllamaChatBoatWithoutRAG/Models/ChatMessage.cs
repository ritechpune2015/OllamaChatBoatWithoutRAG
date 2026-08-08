using OllamaChatBoatWithoutRAG.Enums;

namespace OllamaChatBoatWithoutRAG.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public ChatRole Role { get; set; }
        public string Content { get; set; } = "";
        public DateTime CreatedOn { get; set; }
    }

}
