namespace TimedChecker.Job.Configuration;

public record JobSettings(string CronSchedule, bool RunOnStartup, bool Headless)
{
    public JobSettings() : this(string.Empty, false, true)
    {
    }
}