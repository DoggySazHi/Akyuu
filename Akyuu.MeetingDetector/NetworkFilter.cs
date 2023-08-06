using System.Runtime.Versioning;
using System.Timers;
using Microsoft.Extensions.Logging;

namespace Akyuu.MeetingDetector;

internal class Sample
{
    public Dictionary<(byte, byte), int> PacketsReceived { get; set; } = new();
}

[SupportedOSPlatform("windows")]
public class NetworkFilter : IDisposable
{
    private readonly Dictionary<(byte, byte), byte> _ips = new();
    private readonly Dictionary<byte, string> _services = new();

    private const int WindowSeconds = 15; // How many samples the queue can have
    private const int PacketStartThreshold = 10;  // Total packets in accumulator to trigger meeting start
    private const int PacketEndThreshold = 5;  // Total packets in accumulator to trigger meeting end

    private readonly object _accumulatorLock = new();
    private readonly Dictionary<(byte, byte), int> _accumulator = new();
    private readonly Queue<Sample> _samples = new();
    private readonly HashSet<byte> _meetingsStarted = new();
    private Sample _currentSample = new ();

    private readonly NetworkListener _listener;
    private readonly System.Timers.Timer _timer;

    public event EventHandler<MeetingEventArgs>? MeetingStarted; 
    public event EventHandler<MeetingEventArgs>? MeetingEnded; 
    
    public NetworkFilter(ILogger<NetworkFilter>? logger = null)
    {
        _listener = new NetworkListener(logger);
        _listener.UdpPacketReceived += ListenerOnUdpPacketReceived;
        NetworkFilterConsts.AddAllIPs(_ips, _services);
        _timer = new System.Timers.Timer(TimeSpan.FromSeconds(1))
        {
            AutoReset = true
        };
        _timer.Elapsed += TimerOnElapsed;
        _samples.EnsureCapacity(WindowSeconds);
    }

    public void Start()
    {
        _timer.Start();
        _listener.Start();
    }

    public void Stop()
    {
        _listener.Stop();
        _timer.Stop();
    }

    public void Dispose()
    {
        _timer.Dispose();
    }

    private void ListenerOnUdpPacketReceived(object? sender, UdpPacketReceivedEventArgs e)
    {
        lock (_accumulatorLock)
        {
            var bytes = e.Header.SourceIPAddress.GetAddressBytes();
            if (!_ips.TryGetValue((bytes[0], bytes[1]), out _))
            {
                bytes = e.Header.DestinationIPAddress.GetAddressBytes();
            }

            var shortIp = (bytes[0], bytes[1]);

            _currentSample.PacketsReceived.TryAdd(shortIp, 0);
            _currentSample.PacketsReceived[shortIp] += 1;
        }
    }
    
    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        lock (_accumulatorLock)
        {
            if (_samples.Count >= WindowSeconds)
            {
                var dropped = _samples.Dequeue();
                foreach (var pair in dropped.PacketsReceived)
                {
                    _accumulator[pair.Key] -= pair.Value;
                    
                    var serviceId = _ips[pair.Key];
                    if (_accumulator[pair.Key] <= PacketEndThreshold && _meetingsStarted.Contains(serviceId))
                    {
                        var service = _services[serviceId];

                        _meetingsStarted.Remove(serviceId);
                        MeetingEnded?.Invoke(this, new MeetingEventArgs(pair.Key, serviceId, service));
                    }
                }
            }
            
            _samples.Enqueue(_currentSample);
            foreach (var pair in _currentSample.PacketsReceived)
            {
                _accumulator.TryAdd(pair.Key, 0);
                _accumulator[pair.Key] += pair.Value;
                
                var serviceId = _ips[pair.Key];
                if (_accumulator[pair.Key] >= PacketStartThreshold && !_meetingsStarted.Contains(serviceId))
                {
                    var service = _services[serviceId];
                    
                    _meetingsStarted.Add(serviceId);
                    MeetingStarted?.Invoke(this, new MeetingEventArgs(pair.Key, serviceId, service));
                }
            }
            
            _currentSample = new Sample();
        }
    }
}