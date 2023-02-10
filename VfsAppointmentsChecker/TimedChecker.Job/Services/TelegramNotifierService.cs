using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using TimedChecker.Job.Configuration;

namespace TimedChecker.Job.Services;

public class TelegramNotifierService : INotifierService
{
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
            Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    { "chat_id", chatId },
                    { "text", message }
                };

            string json = JsonConvert.SerializeObject(dictionary);
            var requestData = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"https://api.telegram.org/bot{_configuration.BotId}/sendMessage", requestData);
            var result = await response.Content.ReadAsStringAsync();
        }
    }
}