namespace TimedChecker.Job.Services;

public interface IJobExecutionService
{
    Task TriggerAsync();
    Task StartAsync();
    Task StopAsync();
}