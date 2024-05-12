using System.Net.Http.Headers;
using System.Net.Http.Json;
using MovieHub.Models;
using MovieHub.Models.PrincesTheatre;

namespace MovieHub.Services;

public class PrincesTheatreService(IHttpClientFactory httpClientFactory, IConfiguration configuration): IPrincesTheatreService
{
    public async Task<PrincesTheatreResponse?> GetPrincesTheatreMovies(MovieProvider provider)
    {
        var movieProvider = provider.ToString().ToLower();
        var url = $"https://challenge.lexicondigital.com.au/api/v2/{movieProvider}/movies";
        var httpClient = httpClientFactory.CreateClient();
        var apiKey = configuration["princesTheatreAPIKey"] ?? throw new ArgumentNullException(nameof(configuration));
        
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

        var response = await httpClient.GetAsync(url);
        
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadFromJsonAsync<PrincesTheatreResponse>();
        return content;
    }
}