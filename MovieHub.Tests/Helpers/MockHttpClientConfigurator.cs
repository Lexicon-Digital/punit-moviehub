using System.Net;
using System.Text;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace MovieHub.Tests.Helpers;

public class MockHttpClientConfigurator
{
    public readonly Mock<HttpMessageHandler> MockHttpMessageHandler;
    public readonly HttpClient MockHttpClient;
    public readonly MockHttpClientFactory HttpClientFactory;
    
    public MockHttpClientConfigurator(string uriString)
    {
        MockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(MockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(uriString)
        };
        MockHttpClient = httpClient;
        HttpClientFactory = new MockHttpClientFactory(MockHttpClient);
    }
    
    public class MockHttpClientFactory(HttpClient client) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return client;
        }
    }
    
    public void SetupSendAsyncResponse<T>(T expectedResponse) where T : class?
    {
        var responseContent = JsonConvert.SerializeObject(expectedResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
        };
        SetupSendAsync(responseMessage);
    }

    private void SetupSendAsync(HttpResponseMessage responseMessage)
    {
        MockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);
    }
}