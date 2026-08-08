using OllamaChatBoatWithoutRAG.Models;

namespace OllamaChatBoatWithoutRAG.Interfaces
{
    public interface IConversationService
    {
        Conversation CreateConversation();
        List<Conversation> GetConversations();
        Conversation? GetConversation(Guid id);
        List<ChatMessage> GetMessages(Guid conversationId);
        void AddUserMessage(Guid conversationId, string message);
        void AddAssistantMessage(Guid conversationId, string message);

    }
}
