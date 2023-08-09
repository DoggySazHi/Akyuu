using Newtonsoft.Json;

namespace Akyuu.OBS.Models.OpCodes;

[OpCode(5)]
public class Event : IOpCode
{
    [JsonProperty("eventType")] public string EventType { get; set; } = null!;
    [JsonProperty("eventIntent")] public int EventIntent { get; set; }
    [JsonProperty("eventData", DefaultValueHandling = DefaultValueHandling.Ignore)] public object? EventData { get; set; }
}