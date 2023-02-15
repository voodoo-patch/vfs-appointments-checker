namespace TimedChecker.Bot.Handlers;

public interface ITelegramBotCommandHandler
{
    Task<string> HandleCheckAsync();
    Task<string> HandlePauseAsync();
    Task<string> HandleResumeAsync();
}