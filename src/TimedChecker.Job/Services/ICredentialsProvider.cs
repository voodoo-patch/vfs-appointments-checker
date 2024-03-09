using TimedChecker.Job.Options;

namespace TimedChecker.Job.Services;

public interface ICredentialsProvider
{
    VfsCheckerOptions.AccountCredentials GetAccount();
}