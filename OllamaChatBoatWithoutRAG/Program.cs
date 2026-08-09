using Microsoft.Extensions.Options;
using OllamaChatBoatWithoutRAG.Interfaces;
using OllamaChatBoatWithoutRAG.Options;
using OllamaChatBoatWithoutRAG.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OllamaOptions>(builder.Configuration.GetSection("Ollama"));
builder.Services.Configure<AIProviderOptions>(builder.Configuration.GetSection("AIProvider"));

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ConversationStore>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IChatService,ChatService>();
builder.Services.AddScoped<ILLMFactory, LLMFactory>();
builder.Services.AddScoped<ILLMService, OllamaService>();
builder.Services.AddHttpClient<OllamaService>((serviceProvider, client) =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<OllamaOptions>>().Value;
    client.Timeout = TimeSpan.FromMinutes(10);
    client.BaseAddress = new Uri(options.BaseUrl);
});


var app = builder.Build();
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();
