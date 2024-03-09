using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TimedChecker.Bot.Configuration;
using TimedChecker.Bot.Handlers;

namespace TimedChecker.Bot.HostedServices;

public class TelegramBotMessageReceiver(
    ILogger<TelegramBotMessageReceiver> logger,
    IOptions<TelegramSettings> settingsOptions,
    TelegramBotUpdatesHandler telegramBotUpdatesHandler)
    : BackgroundService, IHealthCheck
{
    private readonly TelegramBotClient _botClient = new(settingsOptions.Value.BotToken);
    private readonly TelegramBotErrorHandler _telegramBotErrorHandler = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) =>
        await ReceiveMessage(stoppingToken);

    private async Task ReceiveMessage(CancellationToken stoppingToken)
    {
        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
        };

        _botClient.StartReceiving(
            telegramBotUpdatesHandler.HandleAsync,
            _telegramBotErrorHandler.HandleAsync,
            receiverOptions,
            stoppingToken
        );

        var me = await _botClient.GetMeAsync(stoppingToken);

        logger.LogInformation($"Start listening for @{me.Username}");
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        try
        {
            _ = await _botClient.GetMeAsync(cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Telegram Bot is unhealthy");
            return HealthCheckResult.Unhealthy(e.Message);
        }
    }
}