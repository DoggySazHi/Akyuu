using Akyuu.OBS.Models.OpCodes;
using Newtonsoft.Json;

namespace Akyuu.OBS.Models;

public class OBSMessage<T> where T : class, IOpCode
{
    [JsonProperty("d")] public T Data { get; set; }
    [JsonProperty("op")] public int Operation { get; set; }

    [JsonConstructor]
    public OBSMessage(T data, int operation)
    {
        Data = data;
        Operation = operation;
    }
}