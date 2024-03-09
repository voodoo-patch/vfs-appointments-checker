using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using TimedChecker.Bot.Configuration;
using TimedChecker.Bot.Handlers;

namespace TimedChecker.Bot.Services;

public class VfsAppointmentCheckerClient(
    HttpClient httpClient,
    IOptions<AppointmentCheckerSettings> settings,
    ILogger<TelegramBotCommandHandler> logger) : IAppointmentCheckerClient, IHealthCheck
{
    public async Task CheckAsync() => await SendAsync(settings.Value.Check);

    public async Task PauseAsync() => await SendAsync(settings.Value.Pause);

    public async Task ResumeAsync() => await SendAsync(settings.Value.Resume);

    private async Task SendAsync(AppointmentCheckerSettings.EndpointSettings endpointSettings)
    {
        logger.LogInformation(
            $"Calling Job API: {endpointSettings.Method} - {endpointSettings.Path}");
        var httpMethod = new HttpMethod(endpointSettings.Method);
        var httpRequestMessage = new HttpRequestMessage(httpMethod, endpointSettings.Path);
        using HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
        response.EnsureSuccessStatusCode();
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        try
        {
            var response = await httpClient.GetAsync("healthz", cancellationToken);
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Health check failed");
            return HealthCheckResult.Unhealthy();
        }
    }
}