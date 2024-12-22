namespace DotBased.AspNet.Authority.Services;

public class AuthorityService
{
    public long GenerateVersion() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}