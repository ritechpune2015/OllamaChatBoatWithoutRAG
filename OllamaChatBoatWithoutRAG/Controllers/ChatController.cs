using Microsoft.AspNetCore.Mvc;
using OllamaChatBoatWithoutRAG.DTOS;
using OllamaChatBoatWithoutRAG.Interfaces;

namespace OllamaChatBoatWithoutRAG.Controllers
{
    public class ChatController : Controller
    {
        private readonly IConversationService _conversationService;
        private readonly IChatService _chatService;
        public ChatController(IConversationService conversationService, IChatService chatService)
        {
            _conversationService = conversationService;
            this._chatService = chatService;
        }

        public IActionResult Index()
        {
            var conversation = _conversationService.CreateConversation();
            return View(conversation);
        }


        [HttpPost]
        public async Task Stream([FromBody] ChatRequest request, CancellationToken cancellationToken)
        {
            Response.ContentType = "text/plain; charset=utf-8";

            try
            {
                await foreach (var token in _chatService.StreamAsync(
                        request.ConversationId,
                        request.Message,
                        cancellationToken))
                {
                    await Response.WriteAsync(
                        token,
                        cancellationToken);

                    await Response.Body.FlushAsync(
                        cancellationToken);
                }
            } catch (Exception) 
            {
                
            }
        }
    }
}