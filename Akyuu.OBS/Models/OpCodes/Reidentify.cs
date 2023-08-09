using Newtonsoft.Json;

namespace Akyuu.OBS.Models.OpCodes;

[OpCode(3)]
public class Reidentify : IOpCode
{
    [JsonProperty("eventSubscriptions", DefaultValueHandling = DefaultValueHandling.Ignore)] public int? EventSubscriptions { get; set; }
}