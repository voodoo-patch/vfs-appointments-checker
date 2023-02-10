using TimedChecker.Job.DI;
using TimedChecker.Job.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
        //bind the settings 
        services.AddSettings();
        services.AddQuartzJob();
        services.AddSingleton<INotifierService, TelegramNotifierService>();
        services.AddSingleton<IRecipientsProvider, TelegramRecipientsProvider>();
        services.AddSingleton<IAppointmentsService, VfsAppointmentsService>();
        services.AddSingleton<SlotsFormatter>();
    })
    .Build();

host.Run();