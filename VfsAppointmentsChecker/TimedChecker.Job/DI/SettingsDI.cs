using TimedChecker.Job.Configuration;

namespace TimedChecker.Job.DI;

public static class SettingsDI
{
    public static void AddSettings(this IServiceCollection services)
    {
        services.AddOptions<TelegramSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(nameof(TelegramSettings)).Bind(settings);
            });
        services.AddOptions<VfsSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(nameof(VfsSettings)).Bind(settings);
            });
    }
}