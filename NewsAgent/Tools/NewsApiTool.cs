using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json;

namespace NewsAgent.Tools
{
    public class NewsApiTool(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _apiKey = configuration["NewsApi:ApiKey"]!;

        [KernelFunction("search_news")]
        [Description("Busca notícias recentes sobre um determinado tema ou assunto")]
        public async Task<string> SearchNewsAsync(
            [Description("Tema ou palavra-chave para buscar notícias")] string query,
            [Description("Quantidade de notícias a retornar (máximo 5)")] int pageSize = 3)
        {
            var client = _httpClientFactory.CreateClient("NewsApi");

            var url = $"/everything?q={Uri.EscapeDataString(query)}" +
                      $"&pageSize={pageSize}" +
                      $"&sortBy=publishedAt" +
                      $"&language=pt" +
                      $"&apiKey={_apiKey}";

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<NewsApiResponse>(json);

            if (result?.Articles == null || result.Articles.Count == 0)
                return "Nenhuma notícia encontrada para o tema informado.";

            // Formata as notícias para o Claude interpretar
            var formatted = result.Articles.Select((a, i) =>
                $"""
            [{i + 1}] {a.Title}
            Fonte: {a.Source?.Name} | Publicado em: {a.PublishedAt:dd/MM/yyyy HH:mm}
            Descrição: {a.Description}
            URL: {a.Url}
            """);

            return string.Join("\n\n", formatted);
        }
    }

    file record NewsApiResponse(List<Article>? Articles);
    file record NewsSource(string? Name);
    file record Article(
        string? Title,
        string? Description,
        string? Url,
        DateTime PublishedAt,
        NewsSource? Source);

}
