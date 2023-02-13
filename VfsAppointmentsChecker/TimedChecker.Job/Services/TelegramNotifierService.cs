using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using TimedChecker.Job.Configuration;

namespace TimedChecker.Job.Services;

public class TelegramNotifierService : INotifierService
{
    private const string DefaultParseMode = "markdown";
    private readonly IRecipientsProvider _recipientsProvider;
    private readonly TelegramSettings _configuration;
    private readonly HttpClient _client;

    public TelegramNotifierService(IHttpClientFactory httpClientFactory,
        IRecipientsProvider recipientsProvider,
        IOptions<TelegramSettings> configuration)
    {
        _recipientsProvider = recipientsProvider;
        _configuration = configuration.Value;
        _client = httpClientFactory.CreateClient();
    }

    public async Task Notify(string message, IEnumerable<string> recipients)
    {
        foreach (var chatId in recipients)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"parse_mode", DefaultParseMode},
                {"chat_id", chatId},
                {"text", message}
            };

            var json = JsonConvert.SerializeObject(dictionary);
            var requestData = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{_configuration.BotApiEndpoint}{_configuration.BotId}/sendMessage",
                requestData);
            var result = await response.Content.ReadAsStringAsync();
        }
    }
}