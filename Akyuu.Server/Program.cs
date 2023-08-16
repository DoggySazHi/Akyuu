using System.Runtime.InteropServices;
using Akyuu.MeetingDetector;
using Akyuu.Server.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddHttpClient();
builder.Services.AddControllers().AddNewtonsoftJson();
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
#if !DEBUG
    builder.Services.AddWindowsService(o => o.ServiceName = "Akyuu");
#endif
    builder.Services.AddSingleton<INetworkFilter, NetworkFilter>();
}
else
{
    throw new NotImplementedException("Meeting detector is currently Windows only");
}

builder.Services.AddSingleton<MeetingWorker>();
builder.Services.AddHostedService<MeetingWorker>(provider => provider.GetService<MeetingWorker>()!);

var app = builder.Build();

await app.RunAsync();