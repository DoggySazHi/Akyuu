using Akyuu.MeetingDetector;
using Akyuu.OBS;
using Akyuu.OBS.Requests;
using Akyuu.Server.Models;

namespace Akyuu.Server.Workers;

public class MeetingWorker : BackgroundService
{
    private readonly ILogger<MeetingWorker> _logger;
    private readonly INetworkFilter _filter;
    private readonly OBSController _obs;

    private bool _useReplayBuffer;
    
    private (byte, byte)? _recordingSourceIp;

    public MeetingWorker(ILogger<MeetingWorker> logger, OBSSettings obsSettings, INetworkFilter filter)
    {
        _logger = logger;
        _filter = filter;
        _filter.MeetingStarted += async (_, e) => await FilterOnMeetingStarted(e);
        _filter.MeetingEnded += async (_, e) => await FilterOnMeetingEnded(e);
        _obs = new OBSController(new OBSConfiguration(obsSettings.Host, obsSettings.Password, obsSettings.Port, obsSettings.Timeout));

        _useReplayBuffer = obsSettings.UseReplayBuffer;
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
            _logger.LogInformation("Stopped recording, file saved to {OutputPath}", result.ResponseData?.OutputPath ?? "<failed to save recording>");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _filter.Start();
        await _obs.Start();
        _logger.LogInformation("Initialized meeting worker");

        if (_useReplayBuffer)
        {
            _logger.LogInformation("Replay buffer configured");
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}