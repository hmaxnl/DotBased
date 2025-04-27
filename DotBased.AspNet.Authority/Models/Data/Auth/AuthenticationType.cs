namespace DotBased.AspNet.Authority.Models.Data.Auth;

public class AuthenticationType
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public bool Redirects { get; set; }
    public AuthenticationTypePaths Paths { get; set; } = new();
}

public class AuthenticationTypePaths
{
    public string Login { get; set; } = string.Empty;
    public string Logout { get; set; } = string.Empty;
}