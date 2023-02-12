using TimedChecker.Job.DI;
using TimedChecker.Job.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
        services
            .AddHttpClient()
            .AddSettings(hostContext.Configuration)
            .AddQuartzJob(hostContext.Configuration)
            .AddSingleton<INotifierService, TelegramNotifierService>()
            .AddSingleton<IRecipientsProvider, TelegramRecipientsProvider>()
            .AddSingleton<IAppointmentsService, VfsAppointmentsService>()
            .AddSingleton<SlotsFormatter>()
    )
    .Build();

host.Run();