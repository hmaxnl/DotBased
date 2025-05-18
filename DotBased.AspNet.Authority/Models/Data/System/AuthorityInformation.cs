using DotBased.AspNet.Authority.Models.Data.Auth;
using DotBased.AspNet.Authority.Models.Options.Auth;

namespace DotBased.AspNet.Authority.Models.Data.System;

public class AuthorityInformation
{
    public string ServerName { get; set; } = "Authority.Server";
    public bool IsAuthenticated { get; set; }
    public List<AuthenticationType> AuthenticationTypes { get; set; } = [];
    public List<AuthenticationSessionType> SessionTypes { get; set; } = [];
    public SchemeInformation? SchemeInformation { get; set; }
    public AuthenticatedInformation? AuthenticatedInformation { get; set; }
}

public class SchemeInformation
{
    public string? DefaultScheme { get; set; }
    public List<SchemeInfo> AvailableSchemes { get; set; } = [];
}

public class AuthenticatedInformation
{
    public string? AuthenticatedScheme { get; set; }
}