using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using MovieHub.Models.PrincesTheatre;

namespace MovieHub.Services;

public class PrincesTheatreService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache): IPrincesTheatreService
{
    public async Task<PrincesTheatreResponse?> GetPrincesTheatreMovies(MovieProvider provider)
    {
        var apiKey = configuration["PrincessTheatre:APIKey"] ?? throw new ArgumentNullException(nameof(configuration));
        var baseUrl = configuration["PrincessTheatre:URL"] ?? throw new ArgumentNullException(nameof(configuration));
        var cacheKey = $"{nameof(GetPrincesTheatreMovies)}/{provider}";
        var movieProvider = provider.ToString().ToLower();
        var url = $"{baseUrl}/api/v2/{movieProvider}/movies";

        var cachedValue = memoryCache.TryGetValue(cacheKey, out PrincesTheatreResponse? cachedResponse);

        if (cachedValue) return cachedResponse;
        
        var httpClient = httpClientFactory.CreateClient();
        
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode) return null;
        
        var content = await response.Content.ReadFromJsonAsync<PrincesTheatreResponse>();

        var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));

        memoryCache.Set(cacheKey, content, cacheEntryOptions);
        
        return content;
    }
}