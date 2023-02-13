namespace TimedChecker.Job.DI;

public static class EndpointsDI
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapPost("/start", StartHandler());
        app.MapPost("/stop", StopHandler());
    }

    private static Func<int> StopHandler()
    {
        return () => 0;
    }

    private static Func<int> StartHandler()
    {
        return () => 0;
    }
}
