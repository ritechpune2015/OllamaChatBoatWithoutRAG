using Microsoft.Extensions.Options;
using OllamaChatBoatWithoutRAG.Interfaces;
using OllamaChatBoatWithoutRAG.Options;

namespace OllamaChatBoatWithoutRAG.Services
{
    public class LLMFactory : ILLMFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AIProviderOptions _options;
        public LLMFactory(IServiceProvider serviceProvider, IOptions<AIProviderOptions> options)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }
        public ILLMService Create()
        {
            switch (_options.Provider)
            {
                case "Ollama":
                    return _serviceProvider
                        .GetRequiredService<OllamaService>();

                //case "OpenAI":
                //    return _serviceProvider
                //        .GetRequiredService<OpenAIService>();
                //case "Gemini":
                //    return _serviceProvider
                //        .GetRequiredService<GeminiService>();

                default:
                    throw new Exception("Invalid Provider");
            }
        }
    }

 }
