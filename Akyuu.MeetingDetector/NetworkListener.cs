using System.Net;
using System.Net.Sockets;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;

namespace Akyuu.MeetingDetector;

/// <summary>
/// Class to sniff UDP packets.
/// </summary>
/// <remarks>
/// I was originally going to rely on window names, but I forgot Discord doesn't have a dedicated meeting window...
/// and tbh I don't want to start a Teams/Zoom meeting every time I wanted to test it
///
/// also i stg this code would be Linux compatible but ig i'd have to use C++ interop
/// </remarks>
[SupportedOSPlatform("windows")]
internal class NetworkListener
{
    private readonly IPAddress _source;
    private readonly Socket _socket;
    private readonly byte[] _buffer = new byte[65535]; // yolo
    private readonly ILogger<NetworkListener>? _logger;

    public event EventHandler<UdpPacketReceivedEventArgs>? UdpPacketReceived;

    public NetworkListener(ILogger<NetworkListener>? logger = null)
    {
        // Get first local IPv4 address.
        _source = Dns.GetHostEntry(string.Empty).AddressList
            .First(o => o.AddressFamily == AddressFamily.InterNetwork);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
        _logger = logger;
    }

    public void Start()
    {
        // SMELL ALL THE PACKETS
        _socket.Bind(new IPEndPoint(_source, 0));
        _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
        _socket.IOControl(IOControlCode.ReceiveAll, new byte[] { 1, 0, 0, 0 }, new byte[] { 1, 0, 0, 0 });
        _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, null);
    }

    public void Stop()
    {
        _socket.Close();
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        // we don't want to get overwhelmed, pause the flow
        try
        {
            var bytesRecieved = _socket.EndReceive(result);
            ProcessPacket(bytesRecieved);
        }
        catch (Exception ex)
        {
            _logger.Error("Failed during packet receive");
            _logger.Error(ex: ex);
        }
        
        try
        {
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, null);
        }
        catch (ObjectDisposedException) { /* ignored in case of shutdown */ }
    }

    private bool IsRemoteIP(IPAddress address)
    {
        // Screw it, check first two bytes
        var sourceBytes = _source.GetAddressBytes();
        var destinationBytes = address.GetAddressBytes();
        return sourceBytes[0] != destinationBytes[0] && sourceBytes[1] != destinationBytes[1];
    }

    private void ProcessPacket(int bytesReceived)
    {
        var header = new StrippedIPHeader(_buffer, bytesReceived);
        
        var isUsefulPacket = 
                header.Protocol == ProtocolType.Udp && // Check if it's a UDP packet
               !header.IsBroadcast() && !header.IsMulticast() && // Check if it's an actual data transmission packet
               (Equals(header.SourceIPAddress, _source) || Equals(header.DestinationIPAddress, _source)) && // Check if src/dest is our computer
               (IsRemoteIP(header.SourceIPAddress) || IsRemoteIP(header.DestinationIPAddress)); // Check if it's not local traffic

        if (!isUsefulPacket) return;
        
        var handler = UdpPacketReceived;
        handler?.Invoke(this, new UdpPacketReceivedEventArgs { Header = header });
    }
}