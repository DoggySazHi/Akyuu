using System.Runtime.InteropServices;
using Akyuu.MeetingDetector;
using Akyuu.Server.Models;
using Akyuu.Server.Workers;

var builder = WebApplication.CreateBuilder(args);

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

AddConfiguration<OBSSettings>("OBS");
builder.Services.AddSingleton<MeetingWorker>();
builder.Services.AddHostedService<MeetingWorker>(provider => provider.GetService<MeetingWorker>()!);

var app = builder.Build();

await app.RunAsync();
return;

void AddConfiguration<T>(string configSectionPath) where T : class
{
    if (builder == null) throw new InvalidOperationException("Unexpected nullity of builder");
    
    builder.Services
        .AddOptions<T>()
        .BindConfiguration(configSectionPath)
        .ValidateDataAnnotations()
        .ValidateOnStart();

    builder.Services.AddScoped<T>();
}