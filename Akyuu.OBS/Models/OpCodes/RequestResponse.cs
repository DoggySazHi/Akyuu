using Newtonsoft.Json;

namespace Akyuu.OBS.Models.OpCodes;

[OpCode(7)]
public class RequestResponse : IOpCode
{
    [JsonProperty("requestType")] public string RequestType { get; set; } = null!;
    [JsonProperty("requestId")] public string RequestId { get; set; } = null!;
    [JsonProperty("requestStatus")] public RequestResponseStatus RequestStatus { get; set; } = null!;
    [JsonProperty("responseData")] public object? ResponseData { get; set; }
}

public class RequestResponseStatus
{
    [JsonProperty("result")] public bool Result { get; set; }
    [JsonProperty("code")] public int Code { get; set; }
    [JsonProperty("comment")] public string? Comment { get; set; }
}