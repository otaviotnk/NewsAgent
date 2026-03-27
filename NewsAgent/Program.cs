using JD.SemanticKernel.Connectors.ClaudeCode;
using NewsAgent.Agent;
using NewsAgent.Tools;

var builder = WebApplication.CreateBuilder(args);

// Configurações
var anthropicApiKey = builder.Configuration["Anthropic:ApiKey"]!;
var anthropicModelId = builder.Configuration["Anthropic:ModelId"]!;

// Semantic Kernel
builder.Services.AddKernel()
    .UseClaudeCodeChatCompletion(
        modelId:anthropicModelId,
        apiKey: anthropicApiKey
    );

// HTTP Client para NewsAPI
builder.Services.AddHttpClient("NewsApi", client =>
{
    var baseUrl = builder.Configuration["NewsApi:BaseUrl"]!;
    client.BaseAddress = new Uri(baseUrl);
});

// Serviços da aplicação
builder.Services.AddScoped<NewsApiTool>();
builder.Services.AddScoped<NewsAgentService>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
