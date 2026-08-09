namespace OllamaChatBoatWithoutRAG.Models
{
    public class LLMResponse
    {
        public string Content
        {
            get;
            set;
        } = "";

        public string Model
        {
            get;
            set;
        } = "";

        public int PromptTokens
        {
            get;
            set;
        }

        public int CompletionTokens
        {
            get;
            set;
        }

        public int TotalTokens
        {
            get;
            set;
        }

        public TimeSpan Duration
        {
            get;
            set;
        }

        public bool Success
        {
            get;
            set;
        }

        public string Error
        {
            get;
            set;
        } = "";

    }
}
