using Newtonsoft.Json;

namespace MovieHub.Models.ChatGPT;

public class ChatbotQueryDto
{
    [JsonProperty("query")] public string Message { get; set; } = string.Empty;
}