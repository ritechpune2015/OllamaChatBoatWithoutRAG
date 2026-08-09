using OllamaChatBoatWithoutRAG.Models;

namespace OllamaChatBoatWithoutRAG.Interfaces
{
    public interface ILLMService
    {
        Task<LLMResponse> ChatAsync(LLMRequest messages);
        IAsyncEnumerable<string> StreamAsync(LLMRequest request, CancellationToken cancellationToken = default);
    }
}
