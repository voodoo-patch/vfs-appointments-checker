using Microsoft.Extensions.Options;
using TimedChecker.Job.Options;

namespace TimedChecker.Job.Services;

public class RoundRobinCredentialsProvider : ICredentialsProvider
{
    private readonly VfsCheckerOptions _vfsSettings;
    private int _accountIterator;
    private readonly int _accountsLength;

    public RoundRobinCredentialsProvider(IOptions<VfsCheckerOptions> vfsSettings)
    {
        _vfsSettings = vfsSettings.Value;
        _accountsLength = _vfsSettings.Accounts.Count();
    }

    public VfsCheckerOptions.AccountCredentials GetAccount()
    {
        int index = _accountIterator++ % _accountsLength;
        return _vfsSettings.Accounts.ElementAt(index);
    }
}