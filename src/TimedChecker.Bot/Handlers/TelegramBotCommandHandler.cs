using TimedChecker.Bot.Services;

namespace TimedChecker.Bot.Handlers;

public class TelegramBotCommandHandler : ITelegramBotCommandHandler
{
    private readonly IAppointmentCheckerClient _appointmentCheckerClient;
    private readonly string _defaultReplyToError = "I'm struggling to contact the Appointment Checker 😣\n" +
                                                   "Try again later.";

    public TelegramBotCommandHandler(IAppointmentCheckerClient appointmentCheckerClient)
    {
        _appointmentCheckerClient = appointmentCheckerClient;
    }

    public async Task<string> HandleCheckAsync()
    {
        try
        {
            await _appointmentCheckerClient.CheckAsync();
            return $"Checking for new appointments...\n" +
                   $"If any available I will notify You 🤞";
        }
        catch (Exception)
        {
            return _defaultReplyToError;
        }
    }

    public async Task<string> HandlePauseAsync()
    {
        try
        {
            await _appointmentCheckerClient.PauseAsync();
            return $"Ok! Just tell me when You want me to {BotCommands.ResumeRoute} checking 😉";
        }
        catch (Exception)
        {
            return _defaultReplyToError;
        }
    }

    public async Task<string> HandleResumeAsync()
    {
        try
        {
            await _appointmentCheckerClient.ResumeAsync();
            return $"Gotcha! Checking will start at the next scheduled time ⏰.\n" +
                   $"If You want to {BotCommands.CheckRoute} now itself just tell me.\n" +
                   $"If any available I will notify You 🤞";
        }
        catch (Exception)
        {
            return _defaultReplyToError;
        }
    }
}