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
    private readonly IHost _host;
    private readonly TelegramBotClient _botClient;
    private readonly TelegramBotUpdatesHandler _telegramBotUpdatesHandler;
    private readonly TelegramBotErrorHandler _telegramBotErrorHandler;

    public TelegramBotMessageReceiver(IHost host,
        IOptions<TelegramSettings> settingsOptions,
        TelegramBotUpdatesHandler telegramBotUpdatesHandler)
    {
        _host = host;
        _botClient = new TelegramBotClient(settingsOptions.Value.BotId);
        _telegramBotUpdatesHandler = telegramBotUpdatesHandler;
        _telegramBotErrorHandler = new TelegramBotErrorHandler();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ReceiveMessage();
        await _host.StopAsync(stoppingToken);
    }

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
        Console.ReadLine();

        // Send cancellation request to stop bot
        cts.Cancel();
    }
}