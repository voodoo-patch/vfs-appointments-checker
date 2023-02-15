using TimedChecker.Job.Services;

namespace TimedChecker.Job.DI;

public static class ServicesDI
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
                .AddSingleton<INotifierService, TelegramNotifierService>()
                .AddSingleton<IRecipientsProvider, TelegramRecipientsProvider>()
                .AddSingleton<IAppointmentsService, VfsAppointmentsService>()
                .AddSingleton<IJobExecutionService, QuartzJobExecutionService>()
                .AddSingleton<ICredentialsProvider, RoundRobinCredentialsProvider>()
            ;
    }
}