namespace Akyuu.MeetingDetector;

/// <summary>
/// Header data when a UDP packet is received.
/// </summary>
public class UdpPacketReceivedEventArgs : EventArgs
{
    public StrippedIPHeader Header { get; set; }

    public UdpPacketReceivedEventArgs(StrippedIPHeader header)
    {
        Header = header;
    }
}