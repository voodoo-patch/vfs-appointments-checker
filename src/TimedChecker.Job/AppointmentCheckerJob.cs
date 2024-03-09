using Quartz;
using TimedChecker.Job.Services;

namespace TimedChecker.Job;

public class AppointmentCheckerJob(
    ILogger<AppointmentCheckerJob> logger,
    INotifier notifier,
    IRecipientsProvider recipientsProvider,
    IAppointmentsService appointmentsService,
    SlotsFormatter slotsFormatter)
    : IJob
{
    public const string Key = nameof(AppointmentCheckerJob);

    public async Task Execute(IJobExecutionContext context) => await CheckAppointmentsAndNotify();

    private async Task CheckAppointmentsAndNotify()
    {
        logger.LogInformation($"{DateTime.UtcNow} - Checking for new appointments");
        var (found, slots) = await appointmentsService.GetSlotsAsync();
        if (found)
            await NotifySuccess(slots);
        else
            logger.LogInformation($"{DateTime.UtcNow} - No appointments found");
    }

    private async Task NotifySuccess(IDictionary<string, string> slots)
    {
        var message = slotsFormatter.Format(slots);
        logger.LogInformation(message);

        var chatIds = await recipientsProvider.GetRecipients();
        await notifier.NotifyAsync(message, chatIds);
    }
}