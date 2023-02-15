using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TimedChecker.Bot.Configuration;
using TimedChecker.Bot.Handlers;

namespace TimedChecker.Bot.HostedServices;

public class TelegramBotMessageReceiver : BackgroundService
{
    private readonly TelegramBotClient _botClient;
    private readonly TelegramBotUpdatesHandler _telegramBotUpdatesHandler;
    private readonly TelegramBotErrorHandler _telegramBotErrorHandler;

    public TelegramBotMessageReceiver(
        IOptions<TelegramSettings> settingsOptions,
        TelegramBotUpdatesHandler telegramBotUpdatesHandler)
    {
        _botClient = new TelegramBotClient(settingsOptions.Value.BotId);
        _telegramBotUpdatesHandler = telegramBotUpdatesHandler;
        _telegramBotErrorHandler = new TelegramBotErrorHandler();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) => await ReceiveMessage();

    private async Task ReceiveMessage()
    {
        using CancellationTokenSource cts = new();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
        };

        _botClient.StartReceiving(
            _telegramBotUpdatesHandler.HandleAsync,
            _telegramBotErrorHandler.HandleAsync,
            receiverOptions,
            cts.Token
        );

        var me = await _botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
    }
}