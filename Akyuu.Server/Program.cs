using Akyuu.Server.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
#if !DEBUG
builder.Services.AddWindowsService(o => o.ServiceName = "Akyuu");
#endif
builder.Services.AddHttpClient();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton<MeetingWorker>();
builder.Services.AddHostedService<MeetingWorker>(provider => provider.GetService<MeetingWorker>()!);

var app = builder.Build();

await app.RunAsync();