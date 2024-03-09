using TimedChecker.Job.Configuration;

namespace TimedChecker.Job.DI;

public static class SettingsDI
{
    public static IServiceCollection AddSettings(this IServiceCollection services,
        IConfiguration hostContextConfiguration)
    {
        return services
            .Configure<TelegramSettings>(
                hostContextConfiguration.GetSection(nameof(TelegramSettings))
            )
            .Configure<VfsSettings>(
                hostContextConfiguration.GetSection(nameof(VfsSettings))
            )
            .Configure<JobSettings>(
                hostContextConfiguration.GetSection(nameof(JobSettings))
            );
    }
}