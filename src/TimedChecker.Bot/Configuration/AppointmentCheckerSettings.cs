namespace TimedChecker.Bot.Configuration;

public record AppointmentCheckerSettings(string ApiBaseLocation,
    AppointmentCheckerSettings.EndpointSettings Check,
    AppointmentCheckerSettings.EndpointSettings Pause,
    AppointmentCheckerSettings.EndpointSettings Resume)
{
    public AppointmentCheckerSettings() : this(string.Empty,
        new EndpointSettings(),
        new EndpointSettings(),
        new EndpointSettings())
    {
    }

    public record EndpointSettings(string Method, string Path)
    {
        public EndpointSettings() : this(String.Empty, String.Empty) { }
    }
}