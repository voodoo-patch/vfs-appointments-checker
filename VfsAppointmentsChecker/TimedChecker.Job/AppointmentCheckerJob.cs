using Quartz;
using TimedChecker.Job.Services;

namespace TimedChecker.Job;

public class AppointmentCheckerJob : IJob
{
    private readonly ILogger<AppointmentCheckerJob> _logger;
    private readonly INotifierService _notifierService;
    private readonly IRecipientsProvider _recipientsProvider;
    private readonly IAppointmentsService _appointmentsService;
    private readonly SlotsFormatter _slotsFormatter;

    public AppointmentCheckerJob(ILogger<AppointmentCheckerJob> logger,
        INotifierService notifierService,
        IRecipientsProvider recipientsProvider,
        IAppointmentsService appointmentsService,
        SlotsFormatter slotsFormatter)
    {
        _logger = logger;
        _notifierService = notifierService;
        _recipientsProvider = recipientsProvider;
        _appointmentsService = appointmentsService;
        _slotsFormatter = slotsFormatter;
    }

    public const string Key = nameof(AppointmentCheckerJob);

    public async Task Execute(IJobExecutionContext context)
    {
        await CheckAppointmentsAndNotify();
    }

    private async Task CheckAppointmentsAndNotify()
    {
        _logger.LogInformation($"{DateTime.UtcNow} - Checking for new appointments");
        var (found, slots) = await _appointmentsService.GetSlots();
        if (found)
            await NotifySuccess(slots);
        else
            _logger.LogInformation($"{DateTime.UtcNow} - No appointments found");
    }

    private async Task NotifySuccess(IDictionary<string, string> slots)
    {
        var message = _slotsFormatter.Format(slots);
        _logger.LogInformation(message);

        var chatIds = await _recipientsProvider.GetRecipients();
        await _notifierService.Notify(message, chatIds);
    }
}