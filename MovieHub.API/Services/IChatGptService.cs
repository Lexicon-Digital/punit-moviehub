namespace MovieHub.Services;

public interface IChatGptService
{
    public Task<Dictionary<string, object>?> GetChatCompletionResponse(string promptMessage);
}