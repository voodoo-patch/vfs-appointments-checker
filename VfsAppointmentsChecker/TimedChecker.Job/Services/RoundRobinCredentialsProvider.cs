using Microsoft.Extensions.Options;
using TimedChecker.Job.Configuration;

namespace TimedChecker.Job.Services;

public class RoundRobinCredentialsProvider : ICredentialsProvider
{
    private readonly VfsSettings _vfsSettings;
    private int _accountIterator;
    private readonly int _accountsLength;

    public RoundRobinCredentialsProvider(IOptions<VfsSettings> vfsSettings)
    {
        _vfsSettings = vfsSettings.Value;
        _accountsLength = _vfsSettings.Accounts.Count();
    }

    public Task<VfsSettings.AccountSettings> GetAccountAsync()
    {
        int index = _accountIterator++ % _accountsLength;
        return Task.FromResult(_vfsSettings.Accounts.ElementAt(index));
    }
}