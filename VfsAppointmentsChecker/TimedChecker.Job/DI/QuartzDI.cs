using Microsoft.Extensions.Options;
using Quartz;
using System.Configuration;
using TimedChecker.Job.Configuration;

namespace TimedChecker.Job.DI;

public static class QuartzDI
{
    public static IServiceCollection AddQuartzJob(this IServiceCollection services,
        IConfiguration hostContextConfiguration)
    {
        return services.AddQuartz(q =>
        {
            var settings = new JobSettings();
            hostContextConfiguration.GetSection(nameof(JobSettings)).Bind(settings);

            q.UseMicrosoftDependencyInjectionJobFactory();
            var jobKey = new JobKey(AppointmentCheckerJob.Key);
            q.AddJob<AppointmentCheckerJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithCronSchedule(settings.CronSchedule,
                    _ => _.WithMisfireHandlingInstructionDoNothing()));

            if (settings.RunOnStartup)
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithSimpleSchedule());
        }).AddQuartzHostedService(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });
    }
}