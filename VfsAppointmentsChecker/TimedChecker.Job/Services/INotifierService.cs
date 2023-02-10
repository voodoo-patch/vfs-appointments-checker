using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimedChecker.Job.Services;

public interface INotifierService
{
    Task Notify(string message, IEnumerable<string> recipients);
}