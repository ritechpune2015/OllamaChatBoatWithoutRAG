using OllamaChatBoatWithoutRAG.Models;

namespace OllamaChatBoatWithoutRAG.Interfaces
{
    public interface ILLMService
    {
        Task<string> ChatAsync(List<ChatMessage> messages);
    }
}
