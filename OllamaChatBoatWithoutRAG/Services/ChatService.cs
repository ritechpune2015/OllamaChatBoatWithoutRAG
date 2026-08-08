using OllamaChatBoatWithoutRAG.DTOS;
using OllamaChatBoatWithoutRAG.Interfaces;

namespace OllamaChatBoatWithoutRAG.Services
{
    public class ChatService
    {
        private readonly IConversationService _conversation;
        private readonly ILLMFactory _factory;
        public ChatService(IConversationService conversation, ILLMFactory factory)
        {
            _conversation = conversation;
            _factory = factory;
        }

        //public async Task<string> AskAsync(Guid conversationId,string question)
        public async Task<string> AskAsync(Guid conversationId, string question)
        {
            _conversation.AddUserMessage(conversationId, question);

            var history = _conversation.GetMessages(conversationId);

            var llm = _factory.Create();

            var answer = await llm.ChatAsync(history);

            _conversation
                   .AddAssistantMessage(
                       conversationId,
                       answer);

            return answer;
        }
    }
}

