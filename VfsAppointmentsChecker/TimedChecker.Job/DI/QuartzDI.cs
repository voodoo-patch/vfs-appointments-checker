using Quartz;

namespace TimedChecker.Job.DI;

public static class QuartzDI
{
    public static void AddQuartzJob(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            var jobKey = new JobKey(nameof(AppointmentCheckerJob));
            q.AddJob<AppointmentCheckerJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithCronSchedule("0 5/30 6-21 ? * 2-6"));

            #if DEBUG
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithSimpleSchedule());
            #endif
        });

        // Quartz.Extensions.Hosting hosting
        services.AddQuartzHostedService(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });
    }
}