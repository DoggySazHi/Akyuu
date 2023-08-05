using System.Net;
using System.Net.Sockets;

namespace Akyuu.MeetingDetector;

// I forgot I'm taking CSE-160 next semester
internal record StrippedIPHeader
{
    public ProtocolType Protocol { get; }
    public IPAddress SourceIPAddress { get; }
    public IPAddress DestinationIPAddress { get; }
    
    public StrippedIPHeader(byte[] buffer, int bytesReceived)
    {
        if (bytesReceived < 20)
            throw new InvalidOperationException("Buffer too small for header data");
        
        // Skip the first 9 bytes
        using var stream = new MemoryStream(buffer, 9, bytesReceived);
        using var reader = new BinaryReader(stream);
        
        Protocol = (ProtocolType) reader.ReadByte();
        reader.ReadInt16(); // We actually don't care about the header checksum lol
        SourceIPAddress = new IPAddress(reader.ReadUInt32());
        DestinationIPAddress = new IPAddress(reader.ReadUInt32());
    }

    public bool IsMulticast()
    {
        var bytes = DestinationIPAddress.GetAddressBytes();
        return bytes[0] >= 224 && bytes[0] <= 239;
    }

    public bool IsBroadcast()
    {
        var bytes = DestinationIPAddress.GetAddressBytes();
        return bytes[0] == 255 && bytes[1] == 255 && bytes[2] == 255 && bytes[3] == 255;
    }
}