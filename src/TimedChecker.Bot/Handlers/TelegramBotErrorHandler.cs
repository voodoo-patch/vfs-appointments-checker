using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace TimedChecker.Bot.Handlers;

public class TelegramBotErrorHandler
{
    public TelegramBotErrorHandler()
    {
    }

    public Task HandleAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}