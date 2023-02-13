namespace TimedChecker.Job.Services;

public interface IJobExecutionService
{
    Task Trigger();
    Task Start();
    Task Stop();
}