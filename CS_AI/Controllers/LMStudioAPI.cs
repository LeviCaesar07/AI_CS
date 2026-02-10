using CS_AI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace LMStudioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly LmStudioService _aiService;

        public AIController(LmStudioService aiService, HttpClient httpClient)
        {
            _aiService = aiService;
            _httpClient = httpClient;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate(string request)
        {
            if (string.IsNullOrWhiteSpace(request))
                return BadRequest("Prompt cannot be empty.");

            var result = await _aiService.GenerateTextAsync(request, "ses8");
            Console.WriteLine(result);
            SendMessage(result);
            return Ok(new { response = result });
        }

        [HttpPost("sendMessage")]
        public void SendMessage(string messages)
        {
            var payload = new
            {
                user_code = "KM5K29126",
                device_id = "D-EN6LD",
                receiver = "26736188207239@lid",
                message = "safdasf",
                secret = "a266b296d199236cc70ee9488f3cde0876e1b41a6bee2b664a56073dae41fa05",
                enableTypingEffect = false
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsync("https://api.kirimi.id/v1/send-message", content);
            Console.WriteLine(response);
            var responseBody =  response;

        }
    }
}
