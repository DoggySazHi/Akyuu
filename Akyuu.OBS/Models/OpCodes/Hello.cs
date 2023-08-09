using Newtonsoft.Json;

namespace Akyuu.OBS.Models.OpCodes;

[OpCode(0)]
public class Hello : IOpCode
{
    [JsonProperty("obsWebSocketVersion")] public string Version { get; set; } = null!;
    [JsonProperty("rpcVersion")] public int RPCVersion { get; set; }
    [JsonProperty("authentication")] public Authentication? Authentication { get; set; }
}

public class Authentication
{
    [JsonProperty("challenge")] public string Challenge { get; set; } = null!;
    [JsonProperty("salt")] public string Salt { get; set; } = null!;
}