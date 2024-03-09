using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimedChecker.Bot.DI;
using TimedChecker.Bot.Handlers;
using TimedChecker.Bot.HostedServices;
using TimedChecker.Bot.Services;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services
            .AddHttpClient()
            .AddSettings(context.Configuration)
            .AddSingleton<TelegramBotUpdatesHandler>()
            .AddSingleton<TelegramBotErrorHandler>()
            .AddSingleton<ITelegramBotCommandHandler, TelegramBotCommandHandler>()
            .AddSingleton<IAppointmentChecker, VfsAppointmentChecker>()
            .AddHostedService<TelegramBotMessageReceiver>();
    })
    .Build()
    .RunAsync();