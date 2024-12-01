namespace DotBased.ASP.Authentication.Configuration;

public class AuthenticationConfiguration
{
    public CacheConfiguration Cache { get; set; } = new();
    public LockoutConfiguration Lockout { get; set; } = new();
    public PasswordConfiguration Password { get; set; } = new();
    public UserConfiguration User { get; set; } = new();
}