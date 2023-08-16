namespace Akyuu.MeetingDetector;

public interface INetworkFilter
{
    event EventHandler<MeetingEventArgs>? MeetingStarted;
    event EventHandler<MeetingEventArgs>? MeetingEnded;
    void Start();
    void Stop();
}