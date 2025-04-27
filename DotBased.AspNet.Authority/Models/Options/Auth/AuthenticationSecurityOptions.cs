namespace DotBased.AspNet.Authority.Models.Options.Auth;

public class AuthenticationSecurityOptions
{
    public SecurityMode SecurityMode { get; set; } = SecurityMode.Normal;
    public List<string> AllowedLoginMethods { get; set; } = ["*"];
}

public enum SecurityMode
{
    Loose = 0,
    Normal = 1,
    Strict = 2
}