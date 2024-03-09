using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TimedChecker.Job.Services;

public class TelegramNotifier(HttpClient httpClient, ILogger<TelegramNotifier> logger)
    : INotifier, IHealthCheck
{
    private const string DefaultParseMode = "markdown";

    public async Task NotifyAsync(string message, IEnumerable<string> recipients)
    {
        foreach (var chatId in recipients)
        {
            var dictionary = new Dictionary<string, string>
            {
                { "parse_mode", DefaultParseMode },
                { "chat_id", chatId },
                { "text", message }
            };

            var json = JsonConvert.SerializeObject(dictionary);
            var requestData = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("sendMessage", requestData);
            response.EnsureSuccessStatusCode();
        }
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        try
        {
            var response = await httpClient.GetAsync("getMe", cancellationToken);
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