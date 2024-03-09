using Moq.Protected;
using System.Net;
using Microsoft.Extensions.Logging;
using TimedChecker.Job.Services;

namespace TimedChecker.Tests;

public class TelegramNotifierTests
{
    private readonly TelegramNotifier _sut;
    private readonly IEnumerable<string> _recipients = new List<string> { "channel-id-1" };

    public TelegramNotifierTests()
    {
        _sut = new TelegramNotifier(GetHttpClientMock(),
            Mock.Of<ILogger<TelegramNotifier>>());
    }

    private HttpClient GetHttpClientMock()
    {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            });
        return new HttpClient(mockHandler.Object);
    }

    [Fact]
    public void Notify_HappyPath()
    {
        _sut.Invoking(_ => _.NotifyAsync("message", _recipients))
            .Should()
            .NotThrowAsync();
    }
}