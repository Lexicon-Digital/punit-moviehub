using Newtonsoft.Json;

namespace MovieHub.Models.ChatGPT;

public class ChatGptQuery
{
    public ChatGptQuery(string schema, string message)
    {
        Messages.Add(new Message
        {
            Role = "user",
            Content = $"Given that I have an SQLite3 Database with the following schema:\n\n ${schema} \n\n" +
                      $"I want you to write me an SQL query that answers the following question. The question comes from an" +
                      $"interpolated string, which comes from a user input. So be mindful that the question may be invalid." +
                      $"Also, very importantly, just give me the output of the query without explanations, because I am going to run this " +
                      $"query against the database. IT'S VERY IMPORTANT THAT YOU ONLY GIVE ME THE QUERY WITHOUT ANY ADDITIONAL TEXT, " +
                      $"COMMENTS OR FORMATTING, BECAUSE YOUR QUERY RESPONSE WILL DIRECTLY BE RUN ON THE DATABASE. " +
                      $"Please be mindful that the question should only return SELECT queries, " +
                      $"and no UPDATE or DELETE queries. Any questions related to updating or deleting anything in the database " +
                      $"should be rejected by returning an error message cast with `AS 'error_message'` at the end. " +
                      $"This is the user input question: \"{message}\""
        });
    }

    [JsonProperty("model")] public string Model { get; set; } = "gpt-4o";

    [JsonProperty("messages")] public List<Message> Messages { get; set; } = [];

    public class Message
    {
        [JsonProperty("role")] public string Role { get; set; } = "user";

        [JsonProperty("content")] public string Content { get; set; } = string.Empty;
    }

    [JsonProperty("temperature")] public double Temperature { get; set; } = 0.7;
}