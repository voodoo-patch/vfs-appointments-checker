using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TimedChecker.Bot.Handlers;

public class TelegramBotUpdatesHandler
{
    private readonly ILogger _logger;
    private readonly ITelegramBotCommandHandler _commandHandler;
    private readonly string _defaultReplyToUnsupportedCommand = "I can help you managing the Vfs appointments checker!\n\n" +
                                                            "You can control me by sending these commands:\n" +
                                                            $"{BotCommands.GetCommandsAsString()}";
    
    public TelegramBotUpdatesHandler(ILogger<TelegramBotUpdatesHandler> logger, ITelegramBotCommandHandler commandHandler)
    {
        _logger = logger;
        _commandHandler = commandHandler;
    }

    public async Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { Text: { } messageText } message)
            return;

        var chatId = message.Chat.Id;

        _logger.LogInformation($"Received a '{messageText}' message in chat {chatId}.");

        string reply = await getReply(messageText);

        await botClient.SendTextMessageAsync(
            chatId,
            reply,
            cancellationToken: cancellationToken);
    }

    private async Task<string> getReply(string messageText) =>
        messageText switch
        {
            BotCommands.Check => await _commandHandler.HandleCheckAsync(),
            BotCommands.Pause => await _commandHandler.HandlePauseAsync(),
            BotCommands.Resume => await _commandHandler.HandleResumeAsync(),
            _ => _defaultReplyToUnsupportedCommand
        };
}