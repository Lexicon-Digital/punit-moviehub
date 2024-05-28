using System.Net;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using MovieHub.Models.PrincesTheatre;
using MovieHub.Services;
using Newtonsoft.Json;

namespace MovieHub.Tests.Services;

public class PrincesTheatreServiceTest
{
    private readonly Mock<IMemoryCache> _mockMemoryCache;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly PrincesTheatreService _service;
    private readonly Dictionary<object, object?> _cacheStore;

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
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        
        var mockConfiguration = new Mock<IConfiguration>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        
        mockConfiguration.Setup(configuration => configuration["PrincessTheatre:APIKey"]).Returns("x-api-key-1234");
        mockConfiguration.Setup(configuration => configuration["PrincessTheatre:URL"]).Returns("https://mock-princess-theatre.url/");

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(mockConfiguration.Object["PrincessTheatre:URL"] ?? string.Empty)
        };
        
        mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var httpClientFactory = new ServiceCollection()
            .AddHttpClient("PrincesTheatreClient")
            .Services
            .AddSingleton<IHttpClientFactory>(new MockHttpClientFactory(httpClient))
            .BuildServiceProvider()
            .GetRequiredService<IHttpClientFactory>();

        _service = new PrincesTheatreService(httpClientFactory, mockConfiguration.Object, _mockMemoryCache.Object);
    }

    [Fact]
    public async Task GetPrincesTheatreMovies_Returns_Movies_When_Called()
    {
        var princesTheatreResponse = _cacheStore["GetPrincesTheatreMovies/Filmworld"];
        
        var responseContent = JsonConvert.SerializeObject(princesTheatreResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
        };
        
        SetupSendAsync(responseMessage);
        SetupMemoryCacheGet();
        SetupMemoryCacheSet();
        
        var response = await _service.GetPrincesTheatreMovies(MovieProvider.Filmworld);

        Assert.NotNull(response);
        Assert.Equal("Film World", response.Provider);
    }
    
    private class MockHttpClientFactory(HttpClient client) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return client;
        }
    }

    private void SetupSendAsync(HttpResponseMessage responseMessage)
    {
        
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);
    }

    private void SetupMemoryCacheGet()
    {
        _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns((object key) =>
        {
            var mockEntry = new Mock<ICacheEntry>();
            mockEntry.SetupAllProperties();
            mockEntry.Setup(entry => entry.Value).Returns(() => _cacheStore[key]);
            mockEntry.Setup(entry => entry.Dispose()).Callback(() => _cacheStore[key] = mockEntry.Object.Value);

            return mockEntry.Object;
        });
    }

    private void SetupMemoryCacheSet()
    {
        _mockMemoryCache.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny!))
            .Returns((object key, out object value) =>
            {
                value = (_cacheStore.TryGetValue(key, out var cachedValue) ? cachedValue : null) ?? "";
                return true;
            });
    }
}