using Newtonsoft.Json;

namespace Akyuu.OpenAI.Models.ChatCompletion;

public class ChatCompletionRequest
{
    [JsonProperty("model")] public string Model { get; set; } = "gpt-3.5-turbo";
    [JsonProperty("messages")] public Message[] Messages { get; set; }

    public ChatCompletionRequest(Message[] messages, string? model = null)
    {
        if (model != null) 
            Model = model;
        
        Messages = messages;
    }
}

public class Message
{
    [JsonProperty("role")] public string Role { get; set; }
    [JsonProperty("content")] public string Content { get; set; }
    [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)] public string? Name { get; set; } 
    [JsonProperty("function_call", DefaultValueHandling = DefaultValueHandling.Ignore)] public FunctionCall? FunctionCall { get; set; }

    public Message(string role, string content, string? name = null, FunctionCall? functionCall = null)
    {
        Role = role;
        Content = content;
        Name = name;
        FunctionCall = functionCall;
    }
}

public class FunctionCall
{
    [JsonProperty("name")] public string Name { get; set; } = null!;
    [JsonProperty("arguments")] public string Arguments { get; set; } = null!;
}