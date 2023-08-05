namespace Akyuu.MeetingDetector;

/// <summary>
/// Class to use hard-coded IPs.
/// </summary>
/// <remarks>
/// Definitely not exhaustive.
/// </remarks>
public static class NetworkFilterConsts
{
    public static void AddAllIPs(HashSet<(byte, byte)> ips)
    {
        AddTeamsIPs(ips);
        AddDiscordIPs(ips);
    }
    
    public static void AddTeamsIPs(HashSet<(byte, byte)> teamsIps)
    {
        teamsIps.Add((13, 107));
        teamsIps.Add((52, 107));
        teamsIps.Add((52, 112));
        teamsIps.Add((52, 113));
        teamsIps.Add((52, 114));
        teamsIps.Add((52, 115));
        teamsIps.Add((52, 120));
        teamsIps.Add((52, 122));
        teamsIps.Add((52, 238));
        teamsIps.Add((52, 244));
    }
    
    public static void AddDiscordIPs(HashSet<(byte, byte)> discordIps)
    {
        discordIps.Add((35, 215));
        discordIps.Add((66, 22));
        discordIps.Add((161, 202));
        discordIps.Add((162, 159));
        discordIps.Add((162, 245));
    }
}