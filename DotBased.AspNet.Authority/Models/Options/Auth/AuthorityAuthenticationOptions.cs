namespace DotBased.AspNet.Authority.Models.Options.Auth;

public class AuthorityAuthenticationOptions
{
    public AuthenticationSecurityOptions Security { get; set; } = new AuthenticationSecurityOptions();
    public SessionOptions Session { get; set; } = new SessionOptions();
    public string DefaultScheme { get; set; } = string.Empty;
    public string DefaultAuthenticateScheme { get; set; } = string.Empty;
    public string DefaultChallengeScheme { get; set; } = string.Empty;
    public string DefaultForbidScheme { get; set; } = string.Empty;
    public string DefaultSignInScheme { get; set; } = string.Empty;
    public string DefaultSignOutScheme { get; set; } = string.Empty;
    /// <summary>
    /// List of schemes that the Authority application will support to authenticate against.
    /// </summary>
    public List<SchemeInfo> SchemeInfoMap { get; set; } = [];
}

public class SchemeInfo
{
    public string Scheme { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SchemeType Type { get; set; }
    public string AuthenticationType { get; set; } = string.Empty;
}

public enum SchemeType
{
    Authentication,
    SessionStore
}