namespace TimedChecker.Bot.Services;

public interface IAppointmentChecker
{
    Task CheckAsync();
    Task PauseAsync();
    Task ResumeAsync();
}