using Microsoft.AspNetCore.Mvc;
using OllamaChatBoatWithoutRAG.DTOS;
using OllamaChatBoatWithoutRAG.Interfaces;
using OllamaChatBoatWithoutRAG.Services;

namespace OllamaChatBoatWithoutRAG.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConversationService _conversationService;
        private readonly ChatService _chatService;
        public HomeController(IConversationService conversationService, ChatService chatService)
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
        public async Task<IActionResult> Send(ChatRequest request)
        {
            var response =
                await _chatService.AskAsync(request.ConversationId, request.Message);
            return Ok(
                        new ChatResponse
                        {
                            Response = response
                        });
        }

    }
}
