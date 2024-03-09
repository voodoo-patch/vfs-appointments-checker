using Microsoft.Extensions.Options;
using TimedChecker.Bot.Configuration;
using TimedChecker.Bot.Handlers;
using TimedChecker.Bot.HostedServices;
using TimedChecker.Bot.Services;

namespace TimedChecker.Bot.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
        services
            .AddSettings()
            .AddTelegramBot()
            .AddAppointmentCheckerClient();
    
    private static IServiceCollection AddSettings(this IServiceCollection services) =>
        services
            .AddOptions<TelegramSettings>()
            .BindConfiguration(TelegramSettings.SectionPath)
            .ValidateDataAnnotations()
            .ValidateOnStart().Services
            .AddOptions<AppointmentCheckerSettings>()
            .BindConfiguration(AppointmentCheckerSettings.SectionPath)
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Services;

    private static IServiceCollection AddTelegramBot(this IServiceCollection services) =>
        services
            .AddTransient<TelegramBotUpdatesHandler>()
            .AddTransient<TelegramBotErrorHandler>()
            .AddTransient<ITelegramBotCommandHandler, TelegramBotCommandHandler>()
            .AddHostedService<TelegramBotMessageReceiver>()
            .AddHealthChecks()
            .AddCheck<TelegramBotMessageReceiver>("Telegram Bot")
            .Services;

    private static IServiceCollection AddAppointmentCheckerClient(this IServiceCollection svc) =>
        svc
            .AddHttpClient<VfsAppointmentCheckerClient>((serviceProvider, client) =>
            {
                var settings = serviceProvider
                    .GetRequiredService<IOptions<AppointmentCheckerSettings>>().Value;

                client.BaseAddress = new Uri(settings.ApiBaseLocation);
            })
            .Services
            .AddTransient<IAppointmentCheckerClient, VfsAppointmentCheckerClient>()
            .AddHealthChecks()
            .AddCheck<VfsAppointmentCheckerClient>("VFS Job API")
            .Services;
}