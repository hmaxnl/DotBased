namespace DotBased.ASP.Auth;

public class BasedAuthConfiguration
{
    /// <summary>
    /// Allow users to registrate a user account.
    /// </summary>
    public bool AllowRegistration { get; set; }
    //TODO: Callback when a user registers, so the application can handle sending emails or generate a code to complete the registration.
    public string LoginPath { get; set; } = string.Empty;
    public string LogoutPath { get; set; } = string.Empty;
    /// <summary>
    /// The max age before a AuthenticationState will expire (default: 7 days).
    /// </summary>
    public TimeSpan AuthenticationStateMaxAgeBeforeExpire { get; set; } = TimeSpan.FromDays(7);
    //TODO: Data seeding
    public Action<AuthService>? SeedData { get; set; }
}