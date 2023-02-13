using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TimedChecker.Bot.Handlers;

public class TelegramBotUpdatesHandler
{
    public TelegramBotUpdatesHandler()
    {
    }

    public async Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] {"Help me", "Call me ☎️"}
        })
        {
            ResizeKeyboard = true
        };
        // Echo received message text
        var sentMessage = await botClient.SendTextMessageAsync(
            chatId,
            "You said:\n" + messageText,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}