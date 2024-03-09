using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TimedChecker.Bot.Configuration;
using TimedChecker.Bot.Handlers;

namespace TimedChecker.Bot.Services;

public class VfsAppointmentChecker : IAppointmentChecker
{
    private readonly ILogger<TelegramBotCommandHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly AppointmentCheckerSettings _settings;

    public VfsAppointmentChecker(IHttpClientFactory httpClientFactory,
        IOptions<AppointmentCheckerSettings> settings,
        ILogger<TelegramBotCommandHandler> logger)
    {
        _logger = logger;
        _settings = settings.Value;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(_settings.ApiBaseLocation);
    }

    public async Task CheckAsync() => await SendAsync(_settings.Check);

    public async Task PauseAsync() => await SendAsync(_settings.Pause);

    public async Task ResumeAsync() => await SendAsync(_settings.Resume);

    private async Task SendAsync(AppointmentCheckerSettings.EndpointSettings endpointSettings)
    {
        _logger.LogInformation($"Calling Job Apis: {endpointSettings.Method} - {endpointSettings.Path}");
        var httpMethod = new HttpMethod(endpointSettings.Method);
        var httpRequestMessage = new HttpRequestMessage(httpMethod, endpointSettings.Path);
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage);
        response.EnsureSuccessStatusCode();
    }
}