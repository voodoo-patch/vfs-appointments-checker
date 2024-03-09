using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace TimedChecker.Bot.Configuration;

public record TelegramSettings(string BotId)
{
    public TelegramSettings() : this(string.Empty)
    {
    }
}