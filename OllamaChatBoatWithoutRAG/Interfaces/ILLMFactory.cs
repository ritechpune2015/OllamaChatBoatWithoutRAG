namespace OllamaChatBoatWithoutRAG.Interfaces
{
    public interface ILLMFactory
    {
        ILLMService Create();
    }
}
