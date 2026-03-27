# 📰 NewsAgent

Agente de inteligência artificial especializado em notícias, construído com **.NET 10**, **Microsoft Semantic Kernel** e o modelo **Claude** da Anthropic. O agente busca notícias em tempo real através da **NewsAPI** e responde perguntas de forma clara e objetiva em português.

---

## ✨ Funcionalidades

- 🤖 **Agente conversacional** baseado no modelo Claude (Anthropic)
- 🔍 **Busca automática de notícias** via NewsAPI com *tool calling* nativo
- 🌐 **API REST** para integração com qualquer cliente
- 📄 **Documentação OpenAPI** integrada
- 🇧🇷 Respostas sempre em **português**, com citação de fonte e data

---

## 🏗️ Arquitetura

```
NewsAgent/
├── Controllers/
│   └── NewsAgentController.cs   # Endpoint REST da API
├── Agent/
│   └── NewsAgentService.cs      # Orquestração do Semantic Kernel + Claude
├── Tools/
│   └── NewsApiTool.cs           # Plugin de busca de notícias (KernelFunction)
├── Models/
│   └── AgentRequest.cs          # Modelos de requisição e resposta
├── Program.cs                   # Configuração e injeção de dependência
└── appsettings.json             # Configurações da aplicação
```

### Fluxo de execução

```
Cliente HTTP
    │
    ▼
POST /api/newsagent/ask
    │
    ▼
NewsAgentController
    │
    ▼
NewsAgentService  ──── Semantic Kernel + Claude ────► Tool Calling (automático)
                                                             │
                                                             ▼
                                                       NewsApiTool
                                                             │
                                                             ▼
                                                         NewsAPI
```

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Finalidade |
|---|---|---|
| .NET | 10 | Framework principal |
| Microsoft Semantic Kernel | 1.74.0 | Orquestração do agente de IA |
| JD.SemanticKernel.Connectors.ClaudeCode | 1.0.18 | Conector para Claude (Anthropic) |
| Microsoft.AspNetCore.OpenApi | 10.0.5 | Documentação OpenAPI |
| Microsoft.Extensions.Http | 10.0.5 | HttpClient Factory |

---

## ⚙️ Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Chave de API da **Anthropic** → [console.anthropic.com](https://console.anthropic.com)
- Chave de API da **NewsAPI** → [newsapi.org](https://newsapi.org)

---

## 🚀 Como executar

### 1. Clone o repositório

```bash
git clone https://github.com/otaviotnk/NewsAgent.git
cd NewsAgent
```

### 2. Configure as chaves de API

Edite o arquivo `NewsAgent/appsettings.json` com suas credenciais:

```json
{
  "Anthropic": {
    "ApiKey": "sua-api-key-aqui",
    "ModelId": "claude-sonnet-4-5"
  },
  "NewsApi": {
    "ApiKey": "sua-newsapi-key-aqui",
    "BaseUrl": "https://newsapi.org/v2"
  }
}
```

> ⚠️ **Nunca** versione suas chaves de API. Utilize variáveis de ambiente ou *User Secrets* em produção.

#### Alternativa: variáveis de ambiente

```bash
$env:Anthropic__ApiKey = "sua-api-key-aqui"
$env:NewsApi__ApiKey   = "sua-newsapi-key-aqui"
```

### 3. Execute a aplicação

```bash
cd NewsAgent
dotnet run
```

A API estará disponível em `https://localhost:{porta}`.

---

## 📡 Endpoints

### `POST /api/newsagent/ask`

Envia uma pergunta ao agente. O agente decide automaticamente quando buscar notícias na NewsAPI para enriquecer a resposta.

**Request body:**

```json
{
  "question": "Quais são as últimas notícias sobre inteligência artificial?"
}
```

**Response `200 OK`:**

```json
{
  "question": "Quais são as últimas notícias sobre inteligência artificial?",
  "answer": "Com base nas notícias mais recentes:\n\n[1] **OpenAI lança novo modelo...**\nFonte: TechCrunch | Publicado em: 20/07/2025 14:30\n..."
}
```

**Response `400 Bad Request`:**

```json
"A pergunta não pode ser vazia."
```

---

## 📖 Documentação OpenAPI

Em ambiente de desenvolvimento, a documentação OpenAPI está disponível em:

```
https://localhost:{porta}/openapi/v1.json
```

---

## 🔧 Como o agente funciona

1. A pergunta do usuário é recebida pelo `NewsAgentController` e repassada ao `NewsAgentService`.
2. O `NewsAgentService` inicializa um `ChatHistory` com um prompt de sistema que instrui o Claude a sempre buscar notícias atualizadas antes de responder.
3. O Semantic Kernel utiliza **Function Choice Behavior Auto**, permitindo que o Claude decida automaticamente quando invocar o plugin `NewsPlugin`.
4. O `NewsApiTool` expõe a função `search_news` como um `KernelFunction`, que consulta a NewsAPI filtrando por idioma português, ordenando pelo mais recente.
5. O Claude recebe os resultados da busca e formula uma resposta em português, citando fontes e datas.

---

## 📁 Estrutura do projeto detalhada

| Arquivo | Descrição |
|---|---|
| `Program.cs` | Registra o Semantic Kernel, connector Claude, HttpClient e serviços |
| `NewsAgentController.cs` | Controller REST com o endpoint `POST /ask` |
| `NewsAgentService.cs` | Configura o `ChatHistory`, o system prompt e executa o agente |
| `NewsApiTool.cs` | Plugin do Kernel que consome a NewsAPI e formata os resultados |
| `AgentRequest.cs` | Records `AgentRequest` e `AgentResponse` usados na API |

---

## 🤝 Contribuindo

1. Faça um *fork* do projeto
2. Crie uma branch para sua feature: `git checkout -b feature/minha-feature`
3. Faça o *commit* das suas alterações: `git commit -m 'feat: minha nova feature'`
4. Faça o *push* para a branch: `git push origin feature/minha-feature`
5. Abra um *Pull Request*

---

## 📜 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
