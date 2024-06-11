using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieHub.Controllers;
using MovieHub.Entities;
using MovieHub.Models.ChatGPT;
using MovieHub.Services;

namespace MovieHub.Tests.Controllers;

public class ChatbotControllerTest
{
    private readonly Mock<IChatGptService> _mockChatGptService;
    private readonly ChatbotController _chatBotController;

    public ChatbotControllerTest()
    {
        _mockChatGptService = new Mock<IChatGptService>();

        var mockHttpContext = new Mock<HttpContext>();
        var response = new Mock<HttpResponse>();
        var headerDictionary = new HeaderDictionary();
        var responseHeaders = new ResponseHeaders(headerDictionary);

        response.SetupGet(httpResponse => httpResponse.Headers).Returns(responseHeaders.Headers);
        mockHttpContext.SetupGet(httpContext => httpContext.Response).Returns(response.Object);
        
        _chatBotController = new ChatbotController(_mockChatGptService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            }
        };
    }
    
    [Fact]
    public async void AskChatBot_Returns_Ok_Result_With_List_Of_Movies()
    {
        const string prompt = "Fetch me all the movies in the database";

        var chatbotQuery = new ChatbotQueryDto
        {
            Message = prompt
        };

        var expectedResponse = new Dictionary<string, object>
        {
            { "_query", "SELECT * FROM Movie;" },
            { "data", new List<Movie>() }
        };

        _mockChatGptService.Setup(service => service.GetChatCompletionResponse(prompt))
            .ReturnsAsync(expectedResponse);

        var result = await _chatBotController.AskChatBot(chatbotQuery);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMovies = Assert.IsType<Dictionary<string, object>>(okResult.Value);
        
        Assert.Equal(expectedResponse, returnedMovies);
    }
    
    [Fact]
    public async void AskChatBot_Returns_Bad_Request_Result()
    {
        const string prompt = "Delete all movies in the database";

        var chatbotQuery = new ChatbotQueryDto
        {
            Message = prompt
        };

        const string expectedResponse = "ERROR: Invalid request";

        _mockChatGptService.Setup(service => service.GetChatCompletionResponse(prompt))
            .ThrowsAsync(new BadHttpRequestException(expectedResponse));

        var result = await _chatBotController.AskChatBot(chatbotQuery);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var resultValue = Assert.IsType<string>(badRequestResult.Value);

        Assert.Equal(expectedResponse, resultValue);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
}