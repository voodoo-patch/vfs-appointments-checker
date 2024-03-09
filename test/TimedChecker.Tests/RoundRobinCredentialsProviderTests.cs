using Microsoft.Extensions.Options;
using TimedChecker.Job.Options;
using TimedChecker.Job.Services;

namespace TimedChecker.Tests;

public class RoundRobinCredentialsProviderTests
{
    private readonly RoundRobinCredentialsProvider _sut1Account;
    private readonly RoundRobinCredentialsProvider _sut2Accounts;
    private const string Email1 = "email1";
    private const string Password1 = "password1";
    private const string Email2 = "email2";
    private const string Password2 = "password2";

    public RoundRobinCredentialsProviderTests()
    {
        var options1 = Options.Create(new VfsCheckerOptions
        {
            Accounts = new List<VfsCheckerOptions.AccountCredentials>
                { new() { Email = Email1, Password = Password1 } },
            Browser = new(),
            AuthenticationEndpoint = ""
        });
        var options2 = Options.Create(new VfsCheckerOptions
        {
            Accounts = new List<VfsCheckerOptions.AccountCredentials>
            {
                new() { Email = Email1, Password = Password1 },
                new() { Email = Email2, Password = Password2 }
            },
            Browser = new(),
            AuthenticationEndpoint = ""
        });
        _sut1Account = new RoundRobinCredentialsProvider(options1);
        _sut2Accounts = new RoundRobinCredentialsProvider(options2);
    }

    [Fact]
    public void GetAccountAsync_GivenTwoAccounts_ReturnFirstAndThenSecond()
    {
        var credentials = _sut2Accounts.GetAccount();
        credentials.Email.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(Email1);
        credentials.Password.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(Password1);

        credentials = _sut2Accounts.GetAccount();
        credentials.Email.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(Email2);
        credentials.Password.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(Password2);
    }

    [Fact]
    public void GetAccountAsync_GivenOneAccount_ReturnAccount()
    {
        var credentials = _sut1Account.GetAccount();
        credentials.Email.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(Email1);
        credentials.Password.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(Password1);
    }

    [Fact]
    public void GetAccountAsync_GivenOneAccount_ReturnsSameAccount()
    {
        var credentials1 = _sut1Account.GetAccount();
        var credentials2 = _sut1Account.GetAccount();
        credentials1.Should().BeEquivalentTo(credentials2);
        credentials1.Email.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(Email1);
        credentials1.Password.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(Password1);
    }
}