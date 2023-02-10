using System.Collections.Generic;
using static TimedChecker.Job.Configuration.VfsSettings;

namespace TimedChecker.Job.Configuration;
public record VfsSettings(AccountSettings Account, UrlsSettings Urls)
{
    public VfsSettings() : this(new(string.Empty, string.Empty), new(String.Empty)) { }
    public record AccountSettings(string Email, string Password);

    public record UrlsSettings(string Authentication);

    public const string London = "LONDON";
    public const string Manchester = "MANCHESTER";
}

