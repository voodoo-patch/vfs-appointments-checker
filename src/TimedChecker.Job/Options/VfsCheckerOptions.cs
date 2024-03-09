using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace TimedChecker.Job.Options;

public class VfsCheckerOptions
{
    public const string SectionPath = "VfsChecker";

    [ValidateObjectMembers] public required BrowserOptions Browser { get; init; }

    [Required]
    [MinLength(1)]
    [ValidateEnumeratedItems]
    public required IEnumerable<AccountCredentials> Accounts { get; init; }

    [Required] public required string AuthenticationEndpoint { get; init; }

    public class BrowserOptions
    {
        public bool Headless { get; init; }
    }

    public class AccountCredentials
    {
        [Required] public required string Email { get; init; }
        [Required] public required string Password { get; init; }
    }
}