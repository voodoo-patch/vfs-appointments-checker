namespace TimedChecker.Bot.Handlers;

public static class BotCommands
{
    public const string
        CheckRoute = "/check",
        PauseRoute = "/pause",
        ResumeRoute = "/resume";

    private static readonly BotCommand Check = new(CheckRoute, "Check for new appointments now");
    private static readonly BotCommand Pause = new(PauseRoute, "Stop checking for new appointments");
    private static readonly BotCommand Resume = new(ResumeRoute, "Resume appointments checking");

    private record BotCommand(string Command, string Description);

    public static string GetCommandsAsString() =>
        $"{Check.Command} - {Check.Description}\n" +
        $"{Pause.Command} - {Pause.Description}\n" +
        $"{Resume.Command} - {Resume.Description}";
}