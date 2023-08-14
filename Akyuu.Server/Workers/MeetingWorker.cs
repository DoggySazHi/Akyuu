namespace Akyuu.Server.Workers;

public class MeetingWorker : BackgroundService
{
    private readonly ILogger<MeetingWorker> _logger;

    public MeetingWorker(ILogger<MeetingWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}