namespace DotBased.AspNet.Authority.Models.Options.Auth;

public class AuthorityAuthenticationOptions
{
    public AuthenticationSecurityOptions Security { get; set; } = new AuthenticationSecurityOptions();
    public SessionOptions Session { get; set; } = new SessionOptions();
    public string DefaultScheme { get; set; } = string.Empty;
    public List<SchemeInfo> SchemeMap { get; set; } = [];
}

public class SchemeInfo
{
    public string Scheme { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
    public SchemeType Type { get; set; }
    public string AuthenticationType { get; set; } = string.Empty;
}

public enum SchemeType
{
    Authentication,
    SessionStore
}