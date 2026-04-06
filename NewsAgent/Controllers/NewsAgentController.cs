using Microsoft.AspNetCore.Mvc;
using NewsAgent.Agent;
using NewsAgent.Models;

namespace NewsAgent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsAgentController : ControllerBase
    {
        private readonly NewsAgentService _agentService;

        public NewsAgentController(NewsAgentService agentService)
        {
            _agentService = agentService;
        }

        [HttpPost("ask")]
        [ProducesResponseType(typeof(AgentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Ask([FromBody] AgentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
                return BadRequest("A pergunta não pode ser vazia.");

            var answer = await _agentService.AskAsync(request.Question);

            return Ok(new AgentResponse(request.Question, answer));
        }
    }
}
