using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimedChecker.Job.Services;

public interface IAppointmentsService
{
    Task<(bool, IDictionary<string, string>)> GetSlots();
}