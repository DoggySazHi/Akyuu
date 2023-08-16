using System.Net.WebSockets;
using Akyuu.OBS.Models.OpCodes;
using Websocket.Client;

namespace Akyuu.OBS;

public class OBSController : IDisposable
{
    private readonly OBSConfiguration _configuration;
    private readonly WebsocketClient _client;
    private readonly OBSControllerLogic _logic;

    public OBSController(OBSConfiguration configuration)
    {
        _configuration = configuration;
        _client = new WebsocketClient(configuration.Uri);
        _logic = new OBSControllerLogic(configuration, _client);
    }

    public async Task Start()
    {
        // turns out OBS doesn't like reconnecting, just restart the entire process.
        _client.ErrorReconnectTimeout = null;
        
        _client.IsReconnectionEnabled = _configuration.ReconnectTimeout != null;
        _client.ReconnectTimeout = _configuration.ReconnectTimeout;

        // subscribe shouldn't be long-running, so schedule a task
        _client.MessageReceived.Subscribe(o => Task.Run(() => OnMessage(o)));
        _client.DisconnectionHappened.Subscribe(o => Task.Run(() => OnDisconnect(o)));
        await _client.StartOrFail();
    }

    public async Task Stop()
    {
        await _client.StopOrFail(WebSocketCloseStatus.NormalClosure, "mukyu~");
    }

    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<RequestResponse> SendRequest(Request request)
    {
        return await _logic.SendRequest(request);
    }
    
    public async Task<RequestResponse<T>> SendRequest<T>(Request request)
    {
        var result = await _logic.SendRequest(request);
        return new RequestResponse<T>(result);
    }

    private void OnMessage(ResponseMessage message)
    {
        if (message.MessageType != WebSocketMessageType.Text)
        {
            return;
        }
        
        _logic.OnMessage(message.Text);
    }

    private void OnDisconnect(DisconnectionInfo disconnectionInfo)
    {
        if (disconnectionInfo.CloseStatus != null)
        {
            var code = (int)disconnectionInfo.CloseStatus.Value;
            
            if (code is >= 4000 and <= 4012)
            {
                disconnectionInfo.CancelReconnection = true;
                throw new InvalidOperationException(disconnectionInfo.CloseStatusDescription);
            }
        }
    }
}