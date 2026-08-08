using OllamaChatBoatWithoutRAG.Interfaces;

namespace OllamaChatBoatWithoutRAG.Services
{
    public class LLMFactory : ILLMFactory
    {
        private readonly IServiceProvider _provider;
        public LLMFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ILLMService Create()
        {
            return    _provider.GetRequiredService<OllamaService>();
        }
    }
}
