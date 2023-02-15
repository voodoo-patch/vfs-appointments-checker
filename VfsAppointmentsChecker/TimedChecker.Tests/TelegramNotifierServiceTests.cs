using Microsoft.Extensions.Options;
using Moq.Protected;
using System.Net;
using TimedChecker.Job.Configuration;
using TimedChecker.Job.Services;

namespace TimedChecker.Tests;

public class TelegramNotifierServiceTests
{
    private readonly TelegramNotifierService _sut;
    private readonly IEnumerable<string> _recipients = new List<string> {"channel-id-1"};

    public TelegramNotifierServiceTests()
    {
        _sut = new TelegramNotifierService(GetIHttpClientMock().Object, GetRecipientsProviderMock().Object,
            GetTelegramConfigurationMock().Object);
    }

    private Mock<IOptions<TelegramSettings>> GetTelegramConfigurationMock()
    {
        var configurationMock = new Mock<IOptions<TelegramSettings>>();
        configurationMock.Setup(_ => _.Value)
            .Returns(() => new TelegramSettings(string.Empty, string.Empty, _recipients));
        return configurationMock;
    }

    private Mock<IRecipientsProvider> GetRecipientsProviderMock()
    {
        var recipientsProviderMock = new Mock<IRecipientsProvider>();
        recipientsProviderMock.Setup(_ => _.GetRecipients())
            .Returns(Task.FromResult(_recipients));
        return recipientsProviderMock;
    }

    private Mock<IHttpClientFactory> GetIHttpClientMock()
    {
        var mock = new Mock<HttpMessageHandler>();
        mock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            });
        var httpClient = new HttpClient(mock.Object);
        var ihttpClientMock = new Mock<IHttpClientFactory>();
        ihttpClientMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        return ihttpClientMock;
    }

    [Fact]
    public void Notify_HappyPath()
    {
        _sut.Invoking(_ => _.Notify("message", _recipients))
            .Should()
            .NotThrowAsync();
    }
}