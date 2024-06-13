using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using MovieHub.Models.PrincesTheatre;
using Polly;
using Polly.Extensions.Http;

namespace MovieHub.Services;

public class PrincesTheatreService(
    IHttpClientFactory httpClientFactory, 
    IConfiguration configuration, 
    IMemoryCache memoryCache
): IPrincesTheatreService
{
    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
        .OrResult(message => message.StatusCode == HttpStatusCode.InternalServerError)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    private readonly string _apiKey = configuration["PrincessTheatre:APIKey"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string _baseUrl = configuration["PrincessTheatre:URL"] ?? throw new ArgumentNullException(nameof(configuration));
    
    public async Task<PrincesTheatreResponse?> GetPrincesTheatreMovies(MovieProvider provider)
    {
        var cacheKey = $"{nameof(GetPrincesTheatreMovies)}/{provider}";
        var movieProvider = provider.ToString().ToLower();
        var url = $"{_baseUrl}/api/v2/{movieProvider}/movies";

        var cachedValue = memoryCache.TryGetValue(cacheKey, out PrincesTheatreResponse? cachedResponse);

        if (cachedValue) return cachedResponse;
        
        var httpClient = httpClientFactory.CreateClient();
        
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

        var response = await _retryPolicy.ExecuteAsync(() => httpClient.GetAsync(url));
        
        if (!response.IsSuccessStatusCode) return null;
        
        var content = await response.Content.ReadFromJsonAsync<PrincesTheatreResponse>();
        
        memoryCache.Set(cacheKey, content, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        
        return content;
    }
    
    public async Task<PrincesTheatreCurrencyResponse?> GetPrincesTheatreCurrencyRates()
    {
        var url = $"{_baseUrl}/api/exchangerate/usd";
        
        var httpClient = httpClientFactory.CreateClient();
        
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

        var response = await _retryPolicy.ExecuteAsync(() => httpClient.GetAsync(url));
        
        if (!response.IsSuccessStatusCode) return null;
        
        return await response.Content.ReadFromJsonAsync<PrincesTheatreCurrencyResponse>();
    }
}