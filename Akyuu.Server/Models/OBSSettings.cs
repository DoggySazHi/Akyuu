using System.ComponentModel.DataAnnotations;

namespace Akyuu.Server.Models;

public class OBSSettings
{
    [Required] public string Host { get; set; } = null!;
    public string? Password { get; set; }
    public ushort Port { get; set; } = 4455;
    public uint Timeout { get; set; } = 30000;
    public bool UseReplayBuffer { get; set; } = false;
}