using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimedChecker.Job.Services;

public interface INotifier
{
    Task NotifyAsync(string message, IEnumerable<string> recipients);
}