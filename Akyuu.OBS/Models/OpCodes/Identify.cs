using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Akyuu.OBS.Models.OpCodes;

[OpCode(1)]
public class Identify : IOpCode
{
    [JsonProperty("rpcVersion")] public int RPCVersion { get; set; }
    [JsonProperty("authentication", DefaultValueHandling = DefaultValueHandling.Ignore)] public string? Authentication { get; set; }
    [JsonProperty("eventSubscriptions", DefaultValueHandling = DefaultValueHandling.Ignore)] public int? EventSubscriptions { get; set; }

    [JsonConstructor]
    private Identify(int rpcVersion, string? authentication, int? eventSubscriptions)
    {
        RPCVersion = rpcVersion;
        Authentication = authentication;
        EventSubscriptions = eventSubscriptions;
    }

    public Identify(int rpcVersion, int? eventSubscriptions = null)
    {
        RPCVersion = rpcVersion;
        EventSubscriptions = eventSubscriptions;
    }

    public Identify(int rpcVersion, string password, Authentication challenge, int? eventSubscriptions = null) : this(rpcVersion, eventSubscriptions)
    {
        var firstConcatenation = Encoding.UTF8.GetBytes(password + challenge.Salt);
        var firstHash = SHA256.HashData(firstConcatenation);
        var firstHashEncoded = Convert.ToBase64String(firstHash);
        
        var secondConcatenation = Encoding.UTF8.GetBytes(firstHashEncoded + challenge.Challenge);
        var secondHash = SHA256.HashData(secondConcatenation);
        Authentication = Convert.ToBase64String(secondHash);
    }
}