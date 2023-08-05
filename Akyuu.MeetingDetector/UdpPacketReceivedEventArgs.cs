namespace Akyuu.MeetingDetector;

/// <summary>
/// Header data when a UDP packet is received.
/// </summary>
internal class UdpPacketReceivedEventArgs : EventArgs
{
    public StrippedIPHeader Header { get; set; } = null!;
}