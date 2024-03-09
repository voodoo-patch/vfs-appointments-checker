using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace TimedChecker.Bot.Configuration;

public class AppointmentCheckerSettings
{
    public const string SectionPath = "AppointmentCheckerSettings";
    
    [Required]
    public required string ApiBaseLocation { get; init; }
    
    [Required]
    [ValidateObjectMembers]
    public required EndpointSettings Check { get; init; }
    
    [Required]
    [ValidateObjectMembers]
    public required EndpointSettings Pause { get; init; }
    
    [Required]
    [ValidateObjectMembers]
    public required EndpointSettings Resume { get; init; }

    public class EndpointSettings
    {
        [Required]
        public required string Method { get; init; }
        
        [Required]
        public required string Path { get; init; }
    }
}