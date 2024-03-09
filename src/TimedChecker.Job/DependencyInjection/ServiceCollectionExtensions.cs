using Microsoft.Extensions.Options;
using Quartz;
using Quartz.AspNetCore;
using TimedChecker.Job.Options;
using TimedChecker.Job.Services;

namespace TimedChecker.Job.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
        services
            .AddTelegramNotifier()
            .AddVfsChecker();

    private static IServiceCollection AddTelegramNotifier(this IServiceCollection services) =>
        services
            .AddOptions<TelegramOptions>()
            .BindConfiguration(TelegramOptions.SectionPath)
            .ValidateDataAnnotations()
            .ValidateOnStart().Services
            .AddHttpClient<TelegramNotifier>((serviceProvider, client) =>
            {
                var settings = serviceProvider
                    .GetRequiredService<IOptions<TelegramOptions>>().Value;
                client.BaseAddress = new Uri($"{settings.BotApiEndpoint}{settings.BotToken}/");
            }).Services
            .AddHealthChecks()
            .AddCheck<TelegramNotifier>("Telegram BOT")
            .Services
            .AddTransient<INotifier, TelegramNotifier>()
            .AddTransient<IRecipientsProvider, TelegramRecipientsProvider>();

    private static IServiceCollection AddVfsChecker(this IServiceCollection services) =>
        services
            .AddOptions<VfsCheckerOptions>()
            .BindConfiguration(VfsCheckerOptions.SectionPath)
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Services
            .AddTransient<IAppointmentsService, VfsAppointmentsService>()
            .AddTransient<IJobExecutionService, QuartzJobExecutionService>()
            .AddSingleton<ICredentialsProvider, RoundRobinCredentialsProvider>();

    public static IServiceCollection AddQuartzJob(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddQuartz(q =>
        {
            var jobOptions = new JobOptions();
            configuration.GetSection(JobOptions.SectionPath).Bind(jobOptions);

            var jobKey = new JobKey(AppointmentCheckerJob.Key);
            q.AddJob<AppointmentCheckerJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithCronSchedule(jobOptions.CronSchedule,
                    b => b.WithMisfireHandlingInstructionDoNothing()));

            if (jobOptions.RunOnStartup)
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithSimpleSchedule());
        }).AddQuartzServer(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });
}