namespace NewsAgent.Models
{
    public record AgentRequest(string Question);
    public record AgentResponse(string Question, string Answer);
}
