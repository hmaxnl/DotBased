namespace DotBased.AspNet.Authority.Models.Options.Auth;

public class SessionOptions
{
    public TimeSpan RefreshInterval { get; set; } = TimeSpan.FromMinutes(30);
}