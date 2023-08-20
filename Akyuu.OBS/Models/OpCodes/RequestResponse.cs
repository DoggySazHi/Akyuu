using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Akyuu.OBS.Models.OpCodes;

[OpCode(7)]
public class RequestResponse : IOpCode
{
    [JsonProperty("requestType")] public string RequestType { get; set; } = null!;
    [JsonProperty("requestId")] public string RequestId { get; set; } = null!;
    [JsonProperty("requestStatus")] public RequestResponseStatus RequestStatus { get; set; } = null!;
    [JsonProperty("responseData")] public object? ResponseData { get; set; }
}

public class RequestResponse<T> : RequestResponse
{
    public RequestResponse(RequestResponse original)
    {
        RequestType = original.RequestType;
        RequestId = original.RequestId;
        RequestStatus = original.RequestStatus;
        
        if (original.ResponseData is JObject obj)
        {
            ResponseData = obj.ToObject<T>()!;
        }
        else
        {
            // hopefully this isn't the case
            ResponseData = (T) Convert.ChangeType(original.ResponseData, typeof(T))!;
        }
    }
    
    [JsonProperty("responseData")] public new T ResponseData { get; set; }
}

public class RequestResponseStatus
{
    [JsonProperty("result")] public bool Result { get; set; }
    [JsonProperty("code")] public int Code { get; set; }
    [JsonProperty("comment")] public string? Comment { get; set; }
}