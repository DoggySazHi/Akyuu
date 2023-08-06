namespace Akyuu.MeetingDetector;

/// <summary>
/// Class to use hard-coded IPs.
/// </summary>
/// <remarks>
/// Definitely not exhaustive.
/// </remarks>
internal static class NetworkFilterConsts
{
    public static void AddAllIPs(Dictionary<(byte, byte), byte> ips, Dictionary<byte, string> services)
    {
        AddTeamsIPs(ips, services);
        AddDiscordIPs(ips, services);
    }
    
    public static void AddTeamsIPs(Dictionary<(byte, byte), byte> ips, Dictionary<byte, string> services)
    {
        ips[(13, 107)] = 1;
        ips[(52, 107)] = 1;
        ips[(52, 112)] = 1;
        ips[(52, 113)] = 1;
        ips[(52, 114)] = 1;
        ips[(52, 115)] = 1;
        ips[(52, 120)] = 1;
        ips[(52, 122)] = 1;
        ips[(52, 238)] = 1;
        ips[(52, 244)] = 1;

        services[1] = "Microsoft Teams";
    }
    
    public static void AddDiscordIPs(Dictionary<(byte, byte), byte> ips, Dictionary<byte, string> services)
    {
        ips[(35, 215)] = 2;
        ips[(66, 22)] = 2;
        ips[(161, 202)] = 2;
        ips[(162, 159)] = 2;
        ips[(162, 245)] = 2;

        services[2] = "Discord";
    }
}