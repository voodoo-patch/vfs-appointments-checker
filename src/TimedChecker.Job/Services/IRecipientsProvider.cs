using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimedChecker.Job.Services;

public interface IRecipientsProvider
{
    Task<IEnumerable<string>> GetRecipients();
}