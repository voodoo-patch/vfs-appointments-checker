using System.ComponentModel.DataAnnotations;

namespace TimedChecker.Job.Options;

public class JobOptions
{
    public const string SectionPath = "Job";

    [Required] public string CronSchedule { get; init; } = null!;
    public bool RunOnStartup { get; init; }
}