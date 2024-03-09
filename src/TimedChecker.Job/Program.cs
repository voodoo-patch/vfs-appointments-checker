using TimedChecker.Job.DI;
using TimedChecker.Job.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpClient()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddSettings(builder.Configuration)
    .AddQuartzJob(builder.Configuration)
    .AddServices()
    .AddSingleton<SlotsFormatter>();

var app = builder.Build();

app.EnableSwagger();
app.MapEndpoints();
app.Run();