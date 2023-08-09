using Newtonsoft.Json;

namespace Akyuu.OBS.Models.OpCodes;

[OpCode(2)]
public class Identified : IOpCode
{
    [JsonProperty("negotiatedRpcVersion")] public int NegotiatedRPCVersion { get; set; }
}