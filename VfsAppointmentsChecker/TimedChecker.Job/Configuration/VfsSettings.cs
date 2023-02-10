using System.Collections.Generic;

namespace TimedChecker.Job.Configuration;
public record VfsSettings
{
    public record AccountSettings
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }

    public record UrlsSettings
    {
        public string Authentication { get; init; }
    }

    public const string London = "LONDON";
    public const string Manchester = "MANCHESTER";

    public AccountSettings Account { get; init; }

    public UrlsSettings Urls { get; init; }
}

