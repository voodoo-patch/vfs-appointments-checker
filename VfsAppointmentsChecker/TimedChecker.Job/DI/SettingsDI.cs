using TimedChecker.Job.Configuration;

namespace TimedChecker.Job.DI;

public static class SettingsDI
{
    public static IServiceCollection AddSettings(this IServiceCollection services,
        IConfiguration hostContextConfiguration) =>
        services
            .Configure<TelegramSettings>(
                hostContextConfiguration.GetSection(nameof(TelegramSettings))
            )
            .Configure<VfsSettings>(
                hostContextConfiguration.GetSection(nameof(VfsSettings))
            )
            .Configure<JobSettings>(
                hostContextConfiguration.GetSection(nameof(JobSettings))
            );
    //services.AddOptions<TelegramSettings>()
    //    .Configure<IConfiguration>((settings, configuration) =>
    //    {
    //        configuration.GetSection(nameof(TelegramSettings)).Bind(settings);
    //    });
    //services.AddOptions<VfsSettings>()
    //    .Configure<IConfiguration>((settings, configuration) =>
    //    {
    //        configuration.GetSection(nameof(VfsSettings)).Bind(settings);
    //    });
    //services.AddOptions<JobSettings>()
    //    .Configure<IConfiguration>((settings, configuration) =>
    //    {
    //        configuration.GetSection(nameof(JobSettings)).Bind(settings);
    //    });
    //return services;
    //}
}