using Microsoft.Extensions.Options;
using TimedChecker.Job.Options;
using TimedChecker.Job.Services;


namespace TimedChecker.Tests;

public class TelegramRecipientsProviderTests
{
    private readonly TelegramRecipientsProvider _sut;
    private const string ChannelId = "channel-id-1";
    private static readonly IEnumerable<string> Recipients = new List<string> { ChannelId };

    public TelegramRecipientsProviderTests()
    {
        _sut = new TelegramRecipientsProvider(GetTelegramConfigurationMock().Object);
    }

    private Mock<IOptions<TelegramOptions>> GetTelegramConfigurationMock()
    {
        var configurationMock = new Mock<IOptions<TelegramOptions>>();
        configurationMock.Setup(_ => _.Value)
            .Returns(() => new TelegramOptions
                { BotApiEndpoint = string.Empty, BotToken = string.Empty, Channels = Recipients });
        return configurationMock;
    }

    [Fact]
    public async Task GetRecipients_HappyPath()
    {
        var recipients = await _sut.GetRecipients();
        recipients.Should()
            .HaveCount(1, "")
            .And
            .Contain(ChannelId, "");
    }
}