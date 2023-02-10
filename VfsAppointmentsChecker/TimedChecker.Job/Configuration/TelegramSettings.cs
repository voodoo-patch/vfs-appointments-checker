using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace TimedChecker.Job.Configuration;

public record TelegramSettings
{
    public TelegramSettings() { }
    public TelegramSettings(string BotId, IEnumerable<string> Channels)
    {
        this.BotId = BotId;
        this.Channels = Channels;
    }

    public string BotId { get; init; }
    public IEnumerable<string> Channels { get; init; }

}