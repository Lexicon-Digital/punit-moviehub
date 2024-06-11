using System.IO.Abstractions;
using System.Net.Http.Headers;
using System.Text;
using MovieHub.Models.ChatGPT;
using Newtonsoft.Json;

namespace MovieHub.Services;

public class ChatGptService(
    IHttpClientFactory httpClientFactory, 
    IConfiguration configuration,
    IMovieHubRepository repository,
    IFileSystem fileSystem
): IChatGptService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private readonly IMovieHubRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IFileSystem _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    public async Task<Dictionary<string, object>?> GetChatCompletionResponse(string promptMessage)
    {
        var apiKey = _configuration["ChatGPT:APIKey"] ?? throw new ArgumentNullException(nameof(_configuration));
        var baseUrl = _configuration["ChatGPT:URL"] ?? throw new ArgumentNullException(nameof(_configuration));

        var httpClient = _httpClientFactory.CreateClient();
        
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        
        var schemaFile = Path.Combine(
            _fileSystem.Directory.GetCurrentDirectory(),
            "Scripts",
            "moviehub_schema.sql"
        );

        var schema = await _fileSystem.File.ReadAllTextAsync(schemaFile);

        var chatGptQuery = new ChatGptQuery(schema, promptMessage);

        var jsonContent = JsonConvert.SerializeObject(chatGptQuery);
        var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(baseUrl, requestContent);

        if (!response.IsSuccessStatusCode) return null;
        
        var responseContent = await response.Content.ReadFromJsonAsync<ChatGptCompletionResponse>();

        var rawQuery = responseContent?.Choices.First().Message.Content;

        if (!IsValidQuery(rawQuery)) throw new BadHttpRequestException(rawQuery!);

        return await _repository.RunQuery(rawQuery!);
    }

    private static bool IsValidQuery(string? query)
    {
        return query != null 
               && query.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase) 
               && !query.Contains("ERROR:");
    }
}