using Microsoft.AspNetCore.Http.HttpResults;
using TimedChecker.Job.Services;

namespace TimedChecker.Job.DI;

public static class EndpointsDI
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapPost("/trigger", TriggerHandler);
        app.MapPost("/start", StartHandler);
        app.MapPost("/stop", StopHandler);
    }

    private static readonly Delegate TriggerHandler =
        (IJobExecutionService jobExecutionService) => jobExecutionService.Trigger();

    private static readonly Delegate StopHandler =
        (IJobExecutionService jobExecutionService) => jobExecutionService.Stop();

    private static readonly Delegate StartHandler =
        (IJobExecutionService jobExecutionService) => jobExecutionService.Start();
}