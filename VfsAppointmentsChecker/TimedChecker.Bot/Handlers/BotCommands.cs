namespace TimedChecker.Bot.Handlers;

public static class BotCommands
{
    public const string
        Check = "/check",
        Pause = "/pause",
        Resume = "/resume";

    private static readonly BotCommand _check = new(Check, "Check for new appointments now");
    private static readonly BotCommand _pause = new(Pause, "Stop checking for new appointments");
    private static readonly BotCommand _resume = new(Resume, "Resume appointments checking");

    private record BotCommand(string Command, string Description);

    public static string GetCommandsAsString() =>
        $"{_check.Command} - {_check.Description}\n" +
        $"{_pause.Command} - {_pause.Description}\n" +
        $"{_resume.Command} - {_resume.Description}";
}