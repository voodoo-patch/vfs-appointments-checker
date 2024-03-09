using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace TimedChecker.Bot.Configuration;

public class TelegramSettings
{
    public const string SectionPath = "TelegramSettings";
 
    [Required]
    public required string BotToken { get; init; }
}