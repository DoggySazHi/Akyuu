using Websocket.Client;

namespace Akyuu.OBS;

public class OBSController
{
    private readonly ManualResetEvent _exitSemaphore = new(false);
    private readonly OBSConfiguration _configuration;

    public OBSController(OBSConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Start()
    {
        var url = new Uri($"wss://{_configuration.Host}:{_configuration.Port}");

        using (var client = new WebsocketClient(url))
        {
            client.ReconnectTimeout = TimeSpan.FromSeconds(30);
            client.ReconnectionHappened.Subscribe(info =>
                Log.Information($"Reconnection happened, type: {info.Type}"));

            client.MessageReceived.Subscribe(msg => Log.Information($"Message received: {msg}"));
            client.Start();

            Task.Run(() => client.Send("{ message }"));

            _exitSemaphore.WaitOne();
        }
    }
}