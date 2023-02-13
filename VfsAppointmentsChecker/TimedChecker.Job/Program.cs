using TimedChecker.Job.DI;
using TimedChecker.Job.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpClient()
    .AddSettings(builder.Configuration)
    .AddQuartzJob(builder.Configuration)
    .AddSingleton<INotifierService, TelegramNotifierService>()
    .AddSingleton<IRecipientsProvider, TelegramRecipientsProvider>()
    .AddSingleton<IAppointmentsService, VfsAppointmentsService>()
    .AddSingleton<SlotsFormatter>();

var app = builder.Build();

app.MapEndpoints();
app.Run();