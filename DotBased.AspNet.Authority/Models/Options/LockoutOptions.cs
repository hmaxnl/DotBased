namespace DotBased.AspNet.Authority.Models.Options;

public class LockoutOptions
{
    public bool EnableLockout { get; set; } = true;
    public int FailedAttempts { get; set; } = 3;
    public TimeSpan LockoutTimeout { get; set; } = TimeSpan.FromMinutes(30);
}