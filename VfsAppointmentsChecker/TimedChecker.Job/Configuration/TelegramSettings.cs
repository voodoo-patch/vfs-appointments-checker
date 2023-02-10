using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace TimedChecker.Job.Configuration;

public record TelegramSettings(string BotId, IEnumerable<string> Channels)
{
    public TelegramSettings() : this (string.Empty, new List<string>()){}
}