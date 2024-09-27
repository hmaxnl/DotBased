namespace DotBased.ASP.Auth;

public class BasedAuthConfiguration
{
    /// <summary>
    /// Allow users to registrate.
    /// </summary>
    public bool AllowRegistration { get; set; }
    //TODO: Callback when a user registers, so the application can handle sending emails or generate a code to complete the registration.
    //TODO: Callback for validation email, phone number
    /// <summary>
    /// Allow no passwords on users, not recommended!
    /// </summary>
    public bool AllowEmptyPassword { get; set; } = false;
    /// <summary>
    /// This path is used for redirecting to the login page.
    /// </summary>
    public string LoginPath { get; set; } = string.Empty;
    /// <summary>
    /// The path that will be used if the logout is requested.
    /// </summary>
    public string LogoutPath { get; set; } = string.Empty;
    /// <summary>
    /// The max age before a AuthenticationState will expire (default: 7 days).
    /// </summary>
    public TimeSpan AuthenticationStateMaxAgeBeforeExpire { get; set; } = TimeSpan.FromDays(7);
    /// <summary>
    /// How long a session state will be cached (default: 15 min)
    /// </summary>
    public TimeSpan CachedAuthSessionLifespan { get; set; } = TimeSpan.FromMinutes(15);
    /// <summary>
    /// Can be used to seed a default user and/or group for first time use.
    /// </summary>
    public Action<IAuthDataRepository>? SeedData { get; set; }

    public Type? AuthDataRepositoryType { get; private set; }

    public void SetDataRepositoryType<TDataProviderType>() where TDataProviderType : IAuthDataRepository =>
        AuthDataRepositoryType = typeof(TDataProviderType);

    public Type? SessionStateProviderType { get; private set; }

    public void SetSessionStateProviderType<TSessionStateProviderType>()
        where TSessionStateProviderType : ISessionStateProvider =>
        SessionStateProviderType = typeof(TSessionStateProviderType);
}