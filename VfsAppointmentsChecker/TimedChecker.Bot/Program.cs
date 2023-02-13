using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimedChecker.Bot.DI;
using TimedChecker.Bot.Handlers;
using TimedChecker.Bot.HostedServices;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services
            .AddSettings(context.Configuration)
            .AddSingleton<TelegramBotUpdatesHandler>()
            .AddSingleton<TelegramBotErrorHandler>()
            .AddHostedService<TelegramBotMessageReceiver>();
    })
    .Build()
    .RunAsync();