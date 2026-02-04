using CS_AI.Services;
using LMStudioApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CS_AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly LmStudioService _aiService;
        public WebHookController(LmStudioService aiService,HttpClient httpClient) 
        {
            _aiService = aiService;
            _httpClient = httpClient;
        }

        [HttpPost]
        [Route("api/WebHook")]
        public async Task<IActionResult> Receive([FromBody]WebhookWA data)
        {
            if(data== null)
            {
                return BadRequest();
            }
            switch (data.Event)
            {
                case "message":
                    var result = await _aiService.GenerateTextAsync(data.Message, "Sessiontest4");
                    Console.WriteLine($"message: {data.Message}");
                    Console.WriteLine(result);
                    break;

                case "device.status":
                    Console.WriteLine("status event received");
                    break;

                case "system.notification":
                    Console.WriteLine("notification received");
                    break;

                default:
                    Console.WriteLine($"event: {data.Event}");
                    break;
            }
            return Ok(new { status = "received" });
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage()
        {
            var payload = new
            {
                user_code = "KM5K29126",
                device_id = "D-EN6LD",
                receiver = "6281327522907",
                message = "Pesan langsung tanpa typing effect",
                secret = "a266b296d199236cc70ee9488f3cde0876e1b41a6bee2b664a56073dae41fa05",
                enableTypingEffect = false
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.kirimi.id/v1/send-message", content);

            var responseBody = await response.Content.ReadAsStringAsync();

            return Ok(new
            {
                statusCode = response.StatusCode,
                body = responseBody
            });
        }
    }
}
