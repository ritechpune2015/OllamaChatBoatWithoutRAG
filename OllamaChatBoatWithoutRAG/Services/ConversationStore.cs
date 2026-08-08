using OllamaChatBoatWithoutRAG.Models;

namespace OllamaChatBoatWithoutRAG.Services
{
    public class ConversationStore
    {
        public Dictionary<Guid, Conversation> Conversations = new();
    }
}
