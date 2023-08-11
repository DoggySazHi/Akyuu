using System.Net.Http.Headers;

namespace Akyuu.OpenAI;

public partial class OpenAIClient : IDisposable
{
    private readonly HttpClient _client;

    public OpenAIClient(OpenAIConfiguration configuration, HttpClient? client = null)
    {
        _client = client ?? new HttpClient();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration.APIKey);
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        if (configuration.Organization != null)
        {
            _client.DefaultRequestHeaders.Add("OpenAI-Organization", configuration.Organization);
        }
    }

    public void Dispose()
    {
        _client.Dispose();
        
        GC.SuppressFinalize(this);
    }
}