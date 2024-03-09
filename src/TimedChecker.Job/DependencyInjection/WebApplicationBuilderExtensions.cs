using TimedChecker.Job.Services;

namespace TimedChecker.Job.DependencyInjection;

public static class WebApplicationBuilderExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapPost("/trigger", TriggerHandler);
        app.MapPost("/start", StartHandler);
        app.MapPost("/stop", StopHandler);
    }

    private static readonly Delegate TriggerHandler =
        async (IJobExecutionService jobExecutionService) =>
            await jobExecutionService.TriggerAsync();

    private static readonly Delegate StopHandler =
        async (IJobExecutionService jobExecutionService) => await jobExecutionService.StopAsync();

    private static readonly Delegate StartHandler =
        async (IJobExecutionService jobExecutionService) => await jobExecutionService.StartAsync();
}