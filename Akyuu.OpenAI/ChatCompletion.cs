using System.Text;
using Akyuu.OpenAI.Models.ChatCompletion;
using Newtonsoft.Json;

namespace Akyuu.OpenAI;

public partial class OpenAIClient
{
    public async Task<ChatCompletionResponse> CreateChatCompletion(ChatCompletionRequest request)
    {
        const string url = "https://api.openai.com/v1/chat/completions";
        var response = await _client.PostAsync(url,
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ChatCompletionResponse>(data)!;
    }
}