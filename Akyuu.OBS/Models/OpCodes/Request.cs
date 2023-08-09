using Newtonsoft.Json;

namespace Akyuu.OBS.Models.OpCodes;

[OpCode(6)]
public class Request : IOpCode
{
    [JsonProperty("requestType")] public string RequestType { get; set; }
    [JsonProperty("requestId")] public string RequestId { get; set; }
    [JsonProperty("requestData", DefaultValueHandling = DefaultValueHandling.Ignore)] public object? RequestData { get; set; }

    [JsonConstructor]
    private Request(string requestType, string requestId, object? requestData)
    {
        RequestType = requestType;
        RequestId = requestId;
        RequestData = requestData;
    }
    
    public Request(string requestType, object? requestData = null)
    {
        RequestType = requestType;
        RequestId = Guid.NewGuid().ToString(); // "D" mode
        RequestData = requestData;
    }
}