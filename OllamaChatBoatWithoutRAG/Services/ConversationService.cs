using OllamaChatBoatWithoutRAG.Enums;
using OllamaChatBoatWithoutRAG.Interfaces;
using OllamaChatBoatWithoutRAG.Models;

namespace OllamaChatBoatWithoutRAG.Services
{
    public class ConversationService : IConversationService
    {
        private readonly ConversationStore _store;
        public ConversationService(ConversationStore store)
        {
            _store = store;
        }


        public void AddAssistantMessage(Guid conversationId, string message)
        {
            var conversation = GetConversation(conversationId);
            if (conversation == null)
                return;
            conversation.Messages.Add(new ChatMessage
            {
                Id = Guid.NewGuid(),
                Role = ChatRole.Assistant,
                Content = message,
                CreatedOn = DateTime.Now
            });

        }

        public void AddUserMessage(Guid conversationId, string message)
        {
            var conversation = GetConversation(conversationId);

            if (conversation == null)
                return;

            conversation.Messages.Add(new ChatMessage
            {
                Id = Guid.NewGuid(),
                Role = ChatRole.User,
                Content = message,
                CreatedOn = DateTime.Now
            });

        }

        public Conversation CreateConversation()
        {
            var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "New Chat",
                CreatedOn = DateTime.Now
            };
            _store.Conversations.Add(conversation.Id,conversation);
            return conversation;
        }

        public Conversation? GetConversation(Guid id)
        {
            _store.Conversations.TryGetValue(id, out var conversation);
            return conversation;
        }

        public List<Conversation> GetConversations()
        {
            return _store.Conversations.Values.ToList();
        }

        public List<ChatMessage> GetMessages(Guid conversationId)
        {
            var conversation = GetConversation(conversationId);

            if (conversation == null)
                return new List<ChatMessage>();

            return conversation.Messages
                               .OrderBy(x => x.CreatedOn)
                               .ToList();
        }

    }
}
