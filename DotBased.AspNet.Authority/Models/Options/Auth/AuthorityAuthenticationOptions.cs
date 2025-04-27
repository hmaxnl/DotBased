namespace DotBased.AspNet.Authority.Models.Options.Auth;

public class AuthorityAuthenticationOptions
{
    public AuthenticationSecurityOptions Security { get; set; } = new AuthenticationSecurityOptions();
    public SessionOptions Session { get; set; } = new SessionOptions();
    public string DefaultScheme { get; set; } = string.Empty;
}