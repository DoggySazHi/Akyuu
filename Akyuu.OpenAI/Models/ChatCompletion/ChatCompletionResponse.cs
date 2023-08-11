using Newtonsoft.Json;

namespace Akyuu.OpenAI.Models.ChatCompletion;

public class ChatCompletionResponse
{
    [JsonProperty("id")] public string ID { get; set; } = null!;
    [JsonProperty("object")] public string Object { get; set; } = null!;
    [JsonProperty("created")] public long Created { get; set; }
    [JsonProperty("choices")] public Choices[] Choices { get; set; } = null!;
    [JsonProperty("usage")] public Usage Usage { get; set; } = null!;
}

public class Choices
{
    [JsonProperty("index")] public int Index { get; set; }
    [JsonProperty("message")] public Message Message { get; set; } = null!;
    [JsonProperty("finish_reason")] public string FinishReason { get; set; } = null!;
}

public class Usage
{
    [JsonProperty("prompt_tokens")] public int PromptTokens { get; set; }
    [JsonProperty("completion_tokens")] public int CompletionTokens { get; set; }
    [JsonProperty("total_tokens")] public int TotalTokens { get; set; }
}