using System.Collections.Generic;
using System.Linq;

namespace TimedChecker.Job.Services;

public class SlotsFormatter
{
    public string Format(IDictionary<string, string> slots)
    {
        string appointments = string.Join("\n",
            slots.Select(centre => centre.Key + ": " + centre.Value));
        return $"Found new appointments:\n{appointments}";
    }
}