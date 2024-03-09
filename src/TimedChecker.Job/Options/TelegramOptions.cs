using System.ComponentModel.DataAnnotations;

namespace TimedChecker.Job.Options;

public class TelegramOptions
{
    public const string SectionPath = "Telegram";
    [Required] public required string BotApiEndpoint { get; init; }
    [Required] public required string BotToken { get; init; }
    [Required] [MinLength(1)] public required IEnumerable<string> Channels { get; init; }
}