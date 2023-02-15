using TimedChecker.Job.Configuration;

namespace TimedChecker.Job.Services;

public interface ICredentialsProvider
{
    Task<VfsSettings.AccountSettings> GetAccountAsync();
}