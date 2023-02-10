using Microsoft.Extensions.Options;
using TimedChecker.Job.Configuration;
using TimedChecker.Job.Services;


namespace TimedChecker.Tests
{
    public class TelegramRecipientsProviderTests
    {
        private readonly TelegramRecipientsProvider _sut;
        private static readonly string _channelId = "channel-id-1";
        private static readonly IEnumerable<string> _recipients = new List<string> { _channelId };

        public TelegramRecipientsProviderTests() =>
            _sut = new TelegramRecipientsProvider(GetTelegramConfigurationMock().Object);

        private Mock<IOptions<TelegramSettings>> GetTelegramConfigurationMock()
        {
            Mock<IOptions<TelegramSettings>> configurationMock = new Mock<IOptions<TelegramSettings>>();
            configurationMock.Setup(_ => _.Value)
                .Returns(() => new TelegramSettings(null, _recipients));
            return configurationMock;
        }

        [Fact]
        public void GetRecipients_HappyPath() => _sut.GetRecipients().Result.Should()
            .HaveCount(1, "")
            .And
            .Contain(_channelId, "");
    }
}
