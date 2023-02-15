using TimedChecker.Job.DI;
using TimedChecker.Job.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpClient()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddSettings(builder.Configuration)
    .AddQuartzJob(builder.Configuration)
    .AddSingleton<INotifierService, TelegramNotifierService>()
    .AddSingleton<IRecipientsProvider, TelegramRecipientsProvider>()
    .AddSingleton<IAppointmentsService, VfsAppointmentsService>()
    .AddSingleton<IJobExecutionService, QuartzJobExecutionService>()
    .AddSingleton<SlotsFormatter>();

var app = builder.Build();

app.EnableSwagger();
app.MapEndpoints();
app.Run();