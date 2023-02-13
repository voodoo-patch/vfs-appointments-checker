using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimedChecker.Bot.Configuration;

namespace TimedChecker.Bot.DI;

public static class SettingsDI
{
    public static IServiceCollection AddSettings(this IServiceCollection services,
        IConfiguration hostContextConfiguration)
    {
        return services
            .Configure<TelegramSettings>(
                hostContextConfiguration.GetSection(nameof(TelegramSettings))
            );
    }
}