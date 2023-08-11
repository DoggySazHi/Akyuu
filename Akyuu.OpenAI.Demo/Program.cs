using System.Text.Json.Nodes;
using Akyuu.OpenAI;
using Akyuu.OpenAI.Models.ChatCompletion;

var config = File.ReadAllText("config.json");
var data = JsonNode.Parse(config)!;
var key = data["openai_api_key"]?.ToString() ?? throw new InvalidOperationException("No key found in config");
var org = data["openai_org"]?.ToString();
var client = new OpenAIClient(new OpenAIConfiguration(key, org));

Console.Write("Enter a message: ");
var message = Console.ReadLine() ?? throw new InvalidOperationException("No message received");
var chat = await client.CreateChatCompletion(new ChatCompletionRequest(new []
{
    new Message("system", "You are Akyuu, a chatbot designed to answer questions accurately and succinctly."),
    new Message("user", message)
}));

Console.WriteLine(chat.Choices.Last().Message.Content);
client.Dispose();