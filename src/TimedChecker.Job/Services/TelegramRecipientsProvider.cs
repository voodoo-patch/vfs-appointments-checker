using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TimedChecker.Job.Options;

namespace TimedChecker.Job.Services;

public class TelegramRecipientsProvider(IOptions<TelegramOptions> configuration)
    : IRecipientsProvider
{
    private readonly TelegramOptions _configuration = configuration.Value;

    public Task<IEnumerable<string>> GetRecipients() => Task.FromResult(_configuration.Channels);
}