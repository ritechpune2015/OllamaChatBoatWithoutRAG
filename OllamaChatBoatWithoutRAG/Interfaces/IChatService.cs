using OllamaChatBoatWithoutRAG.Models;

namespace OllamaChatBoatWithoutRAG.Interfaces
{
    public interface IChatService
    {
        Task<LLMResponse> AskAsync(Guid conversationId, string question);
        IAsyncEnumerable<string> StreamAsync(Guid conversationId, string question,
            CancellationToken cancellationToken = default);

    }
}
