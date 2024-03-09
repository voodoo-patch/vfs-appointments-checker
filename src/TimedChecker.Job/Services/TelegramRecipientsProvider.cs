using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TimedChecker.Job.Configuration;

namespace TimedChecker.Job.Services;

public class TelegramRecipientsProvider : IRecipientsProvider
{
    private readonly TelegramSettings _configuration;

    public TelegramRecipientsProvider(IOptions<TelegramSettings> configuration)
    {
        _configuration = configuration.Value;
    }

    public Task<IEnumerable<string>> GetRecipients()
    {
        return Task.FromResult(_configuration.Channels);
    }
}