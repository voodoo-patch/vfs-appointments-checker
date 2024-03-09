namespace TimedChecker.Bot.Services;

public interface IAppointmentCheckerClient
{
    Task CheckAsync();
    Task PauseAsync();
    Task ResumeAsync();
}