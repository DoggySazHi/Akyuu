using System.Reflection;
using Akyuu.OBS.Models;
using Akyuu.OBS.Models.OpCodes;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Akyuu.OBS;

internal class OBSControllerLogic
{
    private static readonly Dictionary<int, MethodInfo> _opCodeHandlers = new();

    private readonly Dictionary<string, TaskCompletionSource<RequestResponse>> _pendingRequests = new();

    static OBSControllerLogic()
    {
        var methods = typeof(OBSControllerLogic).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<OpCodeAttribute>();
            if (attribute != null)
            {
                _opCodeHandlers[attribute.OpCode] = method;
            }
        }
    }
    
    private readonly OBSConfiguration _configuration;
    private readonly WebsocketClient _client;
    
    public OBSControllerLogic(OBSConfiguration configuration, WebsocketClient client)
    {
        _configuration = configuration;
        _client = client;
    }

    public void OnMessage(string data)
    {
        var root = JObject.Parse(data);
#if DEBUG
        Console.WriteLine(root);
#endif
        
        var op = root["op"]?.ToObject<int>();
        if (op == null || root["d"] == null)
            throw new InvalidOperationException("Received null data from websocket");

        var exists = _opCodeHandlers.TryGetValue(op.Value, out var method);
        if (exists && method != null)
        {
            var type = OBSOpCodeLoader.GetDataType(op.Value);
            var obj = root["d"]!.ToObject(type);
            method.Invoke(this, new [] { obj });
        }
    }

    public async Task<RequestResponse> SendRequest(Request request)
    {
        var task = new TaskCompletionSource<RequestResponse>();
        _pendingRequests.Add(request.RequestId, task);
        _client.Send(JsonConvert.SerializeObject(((IOpCode) request).BuildMessage()));
        return await task.Task;
    }

    [OpCode(0)]
    private void OnHello(Hello hello)
    {
        Identify identify;
        
        if (hello.Authentication == null)
        {
            // no password
            identify = new Identify(1);
        }
        else if (_configuration.Password == null)
        {
            throw new InvalidOperationException("Attempted to connect to authenticated server without password");
        }
        else
        {
            identify = new Identify(1, _configuration.Password, hello.Authentication);
        }
        
        _client.Send(JsonConvert.SerializeObject(((IOpCode) identify).BuildMessage()));
    }
    
    [OpCode(7)]
    private void OnEvent(RequestResponse requestResponse)
    {
        if (!_pendingRequests.TryGetValue(requestResponse.RequestId, out var task))
            return;

        task.TrySetResult(requestResponse);
    }
}