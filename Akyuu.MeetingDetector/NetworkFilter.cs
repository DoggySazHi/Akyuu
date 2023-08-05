namespace Akyuu.MeetingDetector;

internal class NetworkFilter
{
    public NetworkFilter(NetworkListener listener)
    {
        listener.UdpPacketReceived += ListenerOnUdpPacketReceived;
    }

    private void ListenerOnUdpPacketReceived(object? sender, UdpPacketReceivedEventArgs e)
    {
        throw new NotImplementedException();
    }
}