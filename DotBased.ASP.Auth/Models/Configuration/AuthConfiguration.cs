namespace DotBased.ASP.Auth.Models.Configuration;

public class AuthConfiguration
{
    public CacheConfiguration Cache { get; set; } = new();
    public LockoutConfiguration Lockout { get; set; } = new();
    public PasswordConfiguration Password { get; set; } = new();
    public ProviderConfiguration Provider { get; set; } = new();
    public RepositoryConfiguration Repository { get; set; } = new();
    public UserConfiguration User { get; set; } = new();
}