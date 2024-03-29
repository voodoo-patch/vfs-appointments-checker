﻿using System.Collections.Generic;
using System.Linq;

namespace TimedChecker.Job.Services;

public class SlotsFormatter
{
    public string Format(IDictionary<string, string> slots)
    {
        var appointments = string.Join("\n\n",
            slots.Select(centre => $"*{centre.Key}*: {centre.Value}"));
        return $"🎉 *Found new appointments!!*\n\n{appointments}";
    }
}