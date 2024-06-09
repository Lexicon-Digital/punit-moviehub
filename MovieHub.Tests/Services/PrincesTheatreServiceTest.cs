using System.Net;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using MovieHub.Models.PrincesTheatre;
using MovieHub.Services;
using MovieHub.Tests.Helpers;
using Newtonsoft.Json;

namespace MovieHub.Tests.Services;

public class PrincesTheatreServiceTest
{
    private readonly Mock<IMemoryCache> _mockMemoryCache;
    private readonly PrincesTheatreService _service;
    private readonly Dictionary<object, object?> _cacheStore;
    private readonly MockHttpClientConfigurator _mockHttpClientConfigurator;

    public PrincesTheatreServiceTest()
    {
        _cacheStore = new Dictionary<object, object?>
        {
            {
                "GetPrincesTheatreMovies/Filmworld", 
                new PrincesTheatreResponse { Provider = "Film World" }
            }
        };
        _mockMemoryCache = new Mock<IMemoryCache>();
        
        var mockConfiguration = new Mock<IConfiguration>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        
        mockConfiguration.Setup(configuration => configuration["PrincessTheatre:APIKey"]).Returns("x-api-key-1234");
        mockConfiguration.Setup(configuration => configuration["PrincessTheatre:URL"]).Returns("https://mock-princess-theatre.url/");

        _mockHttpClientConfigurator = new MockHttpClientConfigurator(mockConfiguration.Object["PrincessTheatre:URL"] ?? string.Empty);
        
        mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(_mockHttpClientConfigurator.MockHttpClient);

        var httpClientFactory = new ServiceCollection()
            .AddHttpClient("PrincesTheatreClient")
            .Services
            .AddSingleton<IHttpClientFactory>(_mockHttpClientConfigurator.HttpClientFactory)
            .BuildServiceProvider()
            .GetRequiredService<IHttpClientFactory>();

        _service = new PrincesTheatreService(httpClientFactory, mockConfiguration.Object, _mockMemoryCache.Object);
    }

    [Fact]
    public async Task GetPrincesTheatreMovies_Returns_Movies_When_Called()
    {
        var princesTheatreResponse = _cacheStore["GetPrincesTheatreMovies/Filmworld"];
        
        _mockHttpClientConfigurator.SetupSendAsyncResponse(princesTheatreResponse);
        SetupMemoryCache(true);
        
        var response = await _service.GetPrincesTheatreMovies(MovieProvider.Filmworld);

        Assert.NotNull(response);
        Assert.Equal("Film World", response.Provider);
    }
    
    [Fact]
    public async Task GetPrincesTheatreMovies_Retries_OnTransientError()
    {
        var princesTheatreResponse = _cacheStore["GetPrincesTheatreMovies/Filmworld"];
        
        var transientResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        var responseContent = JsonConvert.SerializeObject(princesTheatreResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
        };
        
        _mockHttpClientConfigurator.MockHttpMessageHandler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(transientResponse)
            .ReturnsAsync(transientResponse)
            .ReturnsAsync(responseMessage);
        
        SetupMemoryCache(false);
        
        var response = await _service.GetPrincesTheatreMovies(MovieProvider.Filmworld);

        Assert.NotNull(response);
        Assert.Equal("Film World", response.Provider);
        _mockHttpClientConfigurator.MockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(3),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    private void SetupMemoryCache(bool success)
    {
        _mockMemoryCache.Setup(cache => cache.CreateEntry(It.IsAny<object>())).Returns((object key) =>
        {
            var mockEntry = new Mock<ICacheEntry>();
            mockEntry.SetupAllProperties();
            mockEntry.Setup(entry => entry.Value).Returns(() => _cacheStore[key]);
            mockEntry.Setup(entry => entry.Dispose()).Callback(() => _cacheStore[key] = mockEntry.Object.Value);

            return mockEntry.Object;
        });
        
        _mockMemoryCache.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny!))
            .Returns((object key, out object value) =>
            {
                value = (_cacheStore.TryGetValue(key, out var cachedValue) ? cachedValue : null)!;
                return success;
            });
    }
}