using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Models.ChatGPT;
using MovieHub.Services;

namespace MovieHub.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Produces("application/json")]
public class ChatbotController(IChatGptService chatGptService) : Controller
{
    private readonly IChatGptService _chatGptService = chatGptService ?? throw new ArgumentNullException(nameof(chatGptService));

    [HttpPost]
    public async Task<IActionResult> AskChatBot([FromBody] ChatbotQueryDto query)
    {
        try
        {
            var chatGptResponse = await _chatGptService.GetChatCompletionResponse(query.Message);
            return Ok(chatGptResponse);
        }
        catch (BadHttpRequestException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
