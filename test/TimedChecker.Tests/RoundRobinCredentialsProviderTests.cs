using Microsoft.Extensions.Options;
using TimedChecker.Job.Configuration;
using TimedChecker.Job.Services;

namespace TimedChecker.Tests;

public class RoundRobinCredentialsProviderTests
{
    private readonly RoundRobinCredentialsProvider _sut1Account;
    private readonly RoundRobinCredentialsProvider _sut2Accounts;
    private string _email1 = "email1";
    private string _password1 = "password1";
    private string _email2 = "email2";
    private string _password2 = "password2";

    public RoundRobinCredentialsProviderTests()
    {
        _sut1Account = new RoundRobinCredentialsProvider(GetVfsSettings1AccountMock().Object);
        _sut2Accounts = new RoundRobinCredentialsProvider(GetVfsSettings2AccountsMock().Object);
    }

    private Mock<IOptions<VfsSettings>> GetVfsSettings1AccountMock()
    {
        var configurationMock = new Mock<IOptions<VfsSettings>>();
        configurationMock.Setup(_ => _.Value)
            .Returns(() => new VfsSettings(
                new List<VfsSettings.AccountSettings>()
                {
                    new(_email1, _password1)
                }, new VfsSettings.UrlsSettings(string.Empty)));
        return configurationMock;
    }

    private Mock<IOptions<VfsSettings>> GetVfsSettings2AccountsMock()
    {
        var configurationMock = new Mock<IOptions<VfsSettings>>();
        configurationMock.Setup(_ => _.Value)
            .Returns(() => new VfsSettings(
                new List<VfsSettings.AccountSettings>()
                {
                    new(_email1, _password1),
                    new(_email2, _password2)
                }, new VfsSettings.UrlsSettings(string.Empty)));
        return configurationMock;
    }

    [Fact]
    public void GetAccountAsync_GivenTwoAccounts_ReturnFirstAndThenSecond()
    {
        var (email, password) = _sut2Accounts.GetAccountAsync().Result;
        email.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(_email1);
        password.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(_password1);

        (email, password) = _sut2Accounts.GetAccountAsync().Result;
        email.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(_email2);
        password.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(_password2);
    }

    [Fact]
    public void GetAccountAsync_GivenOneAccount_ReturnAccount()
    {
        var (email, password) = _sut1Account.GetAccountAsync().Result;
        email.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(_email1);
        password.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(_password1);
    }

    [Fact]
    public void GetAccountAsync_GivenOneAccount_ReturnsSameAccount()
    {
        var (email, password) = _sut1Account.GetAccountAsync().Result;
        (email, password) = _sut1Account.GetAccountAsync().Result;
        email.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(_email1);
        password.Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(_password1);
    }
}