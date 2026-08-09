using OllamaChatBoatWithoutRAG.DTOS;
using OllamaChatBoatWithoutRAG.Interfaces;
using OllamaChatBoatWithoutRAG.Models;
using System.Runtime.CompilerServices;
using System.Text;

namespace OllamaChatBoatWithoutRAG.Services
{
    public class ChatService : IChatService
    {
        private readonly IConversationService _conversation;
        private readonly ILLMFactory _factory;
        public ChatService(IConversationService conversation, ILLMFactory factory)
        {
            _conversation = conversation;
            _factory = factory;
        }

        //public async Task<string> AskAsync(Guid conversationId,string question)
        public async Task<LLMResponse> AskAsync(Guid conversationId, string question)
        {
            _conversation.AddUserMessage(conversationId, question);

            var history = _conversation.GetMessages(conversationId);

            var llm = _factory.Create();

            var request = new LLMRequest
            {
                Messages = history,
                Temperature = 0.7,
                MaxTokens = 2048,
                Stream = false
            };

            var response = await llm.ChatAsync(request);
            _conversation.AddAssistantMessage(conversationId, response.Content);
            return response;

        }

        public async IAsyncEnumerable<string> StreamAsync(Guid conversationId, string question,
          [EnumeratorCancellation]
        CancellationToken cancellationToken = default)
        {
            // Step 1
            // Add user question to conversation history

            _conversation.AddUserMessage(
                conversationId,
                question);

            // Step 2
            // Get complete conversation history

            var history =
                _conversation.GetMessages(
                    conversationId);

            // Step 3
            // Create LLM request

            var request = new LLMRequest
            {
                Messages = history,
                Temperature = 0.7,
                MaxTokens = 2048,
                Stream = true
            };

            // Step 4
            // Get selected provider

            var llm = _factory.Create();

            // Step 5
            // Accumulate complete response

            var answerBuilder = new StringBuilder();

            // Step 6
            // Receive tokens from LLM

            await foreach (var token in llm.StreamAsync(request, cancellationToken))
            {
                // Add token to complete answer

                answerBuilder.Append(token);

                // Immediately send token
                // to the caller

                yield return token;
            }

            // Step 7
            // Generation completed

            var completeAnswer = answerBuilder.ToString();

            // Step 8
            // Save complete answer
            // into conversation history

            if (!string.IsNullOrWhiteSpace(
                    completeAnswer))
            {
                _conversation
                    .AddAssistantMessage(
                        conversationId,
                        completeAnswer);
            }

        }
    }
}
