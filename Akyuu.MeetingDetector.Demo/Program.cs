using Microsoft.Extensions.Logging;
using Akyuu.MeetingDetector;

#pragma warning disable CA1416

var factory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
var logger = factory.CreateLogger("Akyuu.MeetingDetector.Demo");

logger.LogInformation("Constructing filter");
var filter = new NetworkFilter(factory.CreateLogger<NetworkFilter>());

filter.MeetingStarted += (_, eventArgs) =>
{
    logger.LogInformation("Meeting started: {}: ({}) {}", eventArgs.Ip, eventArgs.ServiceId, eventArgs.ServiceName);
};

filter.MeetingEnded += (_, eventArgs) =>
{
    logger.LogInformation("Meeting ended: {}: ({}) {}", eventArgs.Ip, eventArgs.ServiceId, eventArgs.ServiceName);
};

logger.LogInformation("Starting filter");
filter.Start();

logger.LogInformation("Press any key to quit");
Console.ReadKey(true);

logger.LogInformation("Shutting down");
filter.Stop();
filter.Dispose();

/*logger.LogInformation("Constructing listener");
var listener = new NetworkListener(factory.CreateLogger<NetworkFilter>());

listener.UdpPacketReceived += (_, eventArgs) =>
{
    logger.LogInformation("Packet sniffed: {} -> {} ({})", eventArgs.Header.SourceIPAddress, eventArgs.Header.DestinationIPAddress, eventArgs.Header.Protocol);
};

logger.LogInformation("Starting listener");
listener.Start();

logger.LogInformation("Press any key to quit");
Console.ReadKey(true);

logger.LogInformation("Shutting down");
listener.Stop();
listener.Dispose();*/