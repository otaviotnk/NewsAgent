using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using NewsAgent.Tools;
using System.Text;

namespace NewsAgent.Agent
{
    public class NewsAgentService
    {
        private readonly Kernel _kernel;
        private readonly IChatCompletionService _chatCompletion;

        public NewsAgentService(Kernel kernel, NewsApiTool newsApiTool)
        {
            _kernel = kernel;
            _chatCompletion = kernel.GetRequiredService<IChatCompletionService>();

            // Registra a tool no kernel
            _kernel.Plugins.AddFromObject(newsApiTool, "NewsPlugin");
        }

        public async Task<string> AskAsync(string question)
        {
            // Histórico da conversa
            var chatHistory = new ChatHistory();

            chatHistory.AddSystemMessage(
                """                
                    Você é um assistente especializado em notícias.
                    Sempre que o usuário fizer uma pergunta sobre eventos, fatos ou notícias recentes,
                    utilize a tool de busca de notícias para obter informações atualizadas antes de responder.
                    Responda sempre em português, de forma clara e objetiva.
                    Ao citar notícias, mencione a fonte e a data de publicação.
                
                """);

            chatHistory.AddUserMessage(question);

            // Configuração para habilitar o tool calling automático
            var executionSettings = new PromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            var response = await _chatCompletion.GetChatMessageContentAsync(
                chatHistory,
                executionSettings: executionSettings,
                kernel: _kernel
            );

            return response.Content ?? "Não foi possível gerar uma resposta.";
        }
    }
}
