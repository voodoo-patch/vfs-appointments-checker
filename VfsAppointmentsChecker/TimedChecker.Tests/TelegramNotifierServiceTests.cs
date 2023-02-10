using Microsoft.Extensions.Options;
using Moq.Protected;
using System.Net;
using TimedChecker.Job.Configuration;
using TimedChecker.Job.Services;

namespace TimedChecker.Tests
{
    public class TelegramNotifierServiceTests
    {
        private readonly TelegramNotifierService _sut;
        private readonly IEnumerable<string> _recipients = new List<string>{ "channel-id-1"
    };
        private readonly string _botId = "my-bot-id";

        public TelegramNotifierServiceTests() =>
            _sut = new TelegramNotifierService(GetIHttpClientMock().Object, GetRecipientsProviderMock().Object,
                GetTelegramConfigurationMock().Object);

        private Mock<IOptions<TelegramSettings>> GetTelegramConfigurationMock()
        {
            Mock<IOptions<TelegramSettings>> configurationMock = new Mock<IOptions<TelegramSettings>>();
            configurationMock.Setup(_ => _.Value)
                .Returns(() => new TelegramSettings(_botId, _recipients));
            return configurationMock;
        }

        private Mock<IRecipientsProvider> GetRecipientsProviderMock()
        {
            Mock<IRecipientsProvider> recipientsProviderMock = new Mock<IRecipientsProvider>();
            // ISSUE: method reference
            recipientsProviderMock.Setup(_ => _.GetRecipients())
                .Returns(Task.FromResult(_recipients));
            return recipientsProviderMock;
        }

        private Mock<IHttpClientFactory> GetIHttpClientMock()
        {
            Mock<HttpMessageHandler> mock = new Mock<HttpMessageHandler>();
            mock.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                });
            HttpClient httpClient = new HttpClient(mock.Object);
            Mock<IHttpClientFactory> ihttpClientMock = new Mock<IHttpClientFactory>();
            ihttpClientMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            return ihttpClientMock;
        }

        [Fact]
        public void Notify_HappyPath() => _sut.Invoking(_ => _.Notify("message", _recipients))
            .Should()
            .NotThrowAsync();
    }
}
