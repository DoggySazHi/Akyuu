namespace Akyuu.MeetingDetector;


public class MeetingEventArgs : EventArgs
{
    public (byte, byte) Ip { get; set; }
    public byte ServiceId { get; set; }
    public string ServiceName { get; set; }

    public MeetingEventArgs((byte, byte) ip, byte serviceId, string serviceName)
    {
        Ip = ip;
        ServiceId = serviceId;
        ServiceName = serviceName;
    }
}