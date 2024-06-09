using Newtonsoft.Json;

namespace MovieHub.Models.ChatGPT;

public class ChatGptCompletionResponse
{
    [JsonProperty("id")] public string Id { get; init; } = string.Empty;

    [JsonProperty("object")] public string Object { get; init; } = string.Empty;

    [JsonProperty("model")] public string Model { get; init; } = string.Empty;

    [JsonProperty("choices")] public List<Choice> Choices { get; init; } = [];
    
    public class Choice
    {
        [JsonProperty("index")] public int Index { get; init; }

        [JsonProperty("message")] public ChoiceMessage Message { get; init; } = new();

        [JsonProperty("finish_reason")] public string FinishReason { get; init; } = string.Empty;

        public class ChoiceMessage
        {
            [JsonProperty("role")] public string Role { get; set; } = string.Empty;

            [JsonProperty("content")] public string Content { get; init; } = string.Empty;
        }
    }
}