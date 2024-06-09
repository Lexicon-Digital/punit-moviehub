using System.IO.Abstractions;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MovieHub.Models.ChatGPT;
using MovieHub.Services;
using MovieHub.Tests.Helpers;

namespace MovieHub.Tests.Services;

public class ChatGptServiceTest
{
    private readonly ChatGptService _service;
    private readonly Mock<IMovieHubRepository> _mockRepository;
    private readonly MockHttpClientConfigurator _mockHttpClientConfigurator;

    public ChatGptServiceTest()
    {
        _mockRepository = new Mock<IMovieHubRepository>();
        
        var mockFileSystem = new Mock<IFileSystem>();
        var mockConfiguration = new Mock<IConfiguration>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        
        mockConfiguration.Setup(configuration => configuration["ChatGPT:APIKey"]).Returns("api-key-1234");
        mockConfiguration.Setup(configuration => configuration["ChatGPT:URL"]).Returns("https://mock-chat-gpt.url/");
        mockFileSystem.Setup(fileSystem => fileSystem.Directory.GetCurrentDirectory()).Returns("/test-path");
        mockFileSystem.Setup(fileSystem => fileSystem.File.ReadAllTextAsync(
                It.IsAny<string>(), 
                It.IsAny<Encoding>(), 
                It.IsAny<CancellationToken>())
            )
            .ReturnsAsync("CREATE TABLE Movie (id string);");
        
        _mockHttpClientConfigurator = new MockHttpClientConfigurator(mockConfiguration.Object["ChatGPT:URL"] ?? string.Empty);
        
        mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(_mockHttpClientConfigurator.MockHttpClient);

        var httpClientFactory = new ServiceCollection()
            .AddHttpClient("ChatGptClient")
            .Services
            .AddSingleton<IHttpClientFactory>(_mockHttpClientConfigurator.HttpClientFactory)
            .BuildServiceProvider()
            .GetRequiredService<IHttpClientFactory>();

        _service = new ChatGptService(httpClientFactory, mockConfiguration.Object, _mockRepository.Object, mockFileSystem.Object);
    }

    [Fact]
    public async Task GetChatCompletionResponse_Runs_Raw_Query_When_Called()
    {
        var chatGptResponse = new ChatGptCompletionResponse
        {
            Choices =
            [
                new ChatGptCompletionResponse.Choice
                {
                    Message = new ChatGptCompletionResponse.Choice.ChoiceMessage
                    {
                        Content = "SELECT * FROM Movie;"
                    }
                }
            ]
        };
        
        _mockHttpClientConfigurator.SetupSendAsyncResponse(chatGptResponse);
        
        await _service.GetChatCompletionResponse("Bring me all movies");
        
        _mockRepository.Verify(repository => repository.RunQuery("SELECT * FROM Movie;"), Times.Exactly(1));
    }
    
    [Fact]
    public async Task GetChatCompletionResponse_Rejects_Any_Query_Other_Than_SELECT()
    {
        var chatGptResponse = new ChatGptCompletionResponse
        {
            Choices =
            [
                new ChatGptCompletionResponse.Choice
                {
                    Message = new ChatGptCompletionResponse.Choice.ChoiceMessage
                    {
                        Content = "DELETE FROM Movie WHERE Id = 1;"
                    }
                }
            ]
        };
        
        _mockHttpClientConfigurator.SetupSendAsyncResponse(chatGptResponse);
        
        await Assert.ThrowsAsync<BadHttpRequestException>(() => _service.GetChatCompletionResponse("Delete all movies"));
    }
}