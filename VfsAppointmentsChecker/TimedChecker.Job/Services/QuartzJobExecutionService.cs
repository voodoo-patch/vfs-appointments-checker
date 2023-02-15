using Quartz;
using Quartz.Impl.Matchers;

namespace TimedChecker.Job.Services;

public class QuartzJobExecutionService : IJobExecutionService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly JobKey _jobKey;

    public QuartzJobExecutionService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
        _jobKey = new JobKey(AppointmentCheckerJob.Key);
    }

    public async Task Trigger()
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.TriggerJob(_jobKey);
    }

    public async Task Start()
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.Start();
    }

    public async Task Stop()
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.Standby();
    }
}