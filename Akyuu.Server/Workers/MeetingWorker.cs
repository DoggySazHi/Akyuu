using Akyuu.MeetingDetector;
using Akyuu.OBS;
using Akyuu.OBS.Requests;

namespace Akyuu.Server.Workers;

public class MeetingWorker : BackgroundService
{
    private readonly ILogger<MeetingWorker> _logger;
    private readonly INetworkFilter _filter;
    private readonly OBSController _obs;

    private bool _useReplayBuffer;
    
    private (byte, byte)? _recordingSourceIp;

    public MeetingWorker(ILogger<MeetingWorker> logger, IConfiguration config, INetworkFilter filter)
    {
        _logger = logger;
        _filter = filter;
        _filter.MeetingStarted += async (_, e) => await FilterOnMeetingStarted(e);
        _filter.MeetingEnded += async (_, e) => await FilterOnMeetingEnded(e);
        _obs = new OBSController(new OBSConfiguration(
            config["OBS:Host"] ?? throw new InvalidOperationException(),
            config["OBS:Password"],
            config["OBS:Port"] != null ? ushort.Parse(config["OBS:Port"]!) : (ushort) 4455,
            config["OBS:Timeout"] != null ? uint.Parse(config["OBS:Timeout"]!) : 30000
        ));

        _useReplayBuffer = config.GetValue("OBS:UseReplayBuffer", false);
    }

    private async Task FilterOnMeetingStarted(MeetingEventArgs e)
    {
        _logger.LogInformation("Meeting ({}) started on IP {}", e.ServiceName, e.Ip);
        if (_recordingSourceIp == null)
        {
            _recordingSourceIp = e.Ip;
            await _obs.StartRecording();
            _logger.LogInformation("Started recording");
        }
    }
    
    private async Task FilterOnMeetingEnded(MeetingEventArgs e)
    {
        _logger.LogInformation("Meeting ({}) ended on IP {}", e.ServiceName, e.Ip);
        if (_recordingSourceIp == e.Ip)
        {
            _recordingSourceIp = null;
            
            var result = await _obs.StopRecording();
            if (result.ResponseData != null)
            {
                dynamic response = result.ResponseData;
                string outputPath = response.outputPath;
                _logger.LogInformation("Stopped recording, file saved to {OutputPath}", outputPath);
            }
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _filter.Start();
        await _obs.Start();
        _logger.LogInformation("Initialized meeting worker");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}