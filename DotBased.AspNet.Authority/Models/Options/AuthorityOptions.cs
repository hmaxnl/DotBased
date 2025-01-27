namespace DotBased.AspNet.Authority.Models.Options;

public class AuthorityOptions
{
    public LockdownOptions Lockdown { get; set; } = new();
    public LockoutOptions Lockout { get; set; } = new();
    public PasswordOptions Password { get; set; } = new();
    public ProviderOptions Provider { get; set; } = new();
    public RepositoryOptions Repository { get; set; } = new();
    public UserOptions User { get; set; } = new();
}