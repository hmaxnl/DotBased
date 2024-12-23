namespace DotBased.AspNet.Authority.Services;

public class AuthorityManager<TData>
{
    public long GenerateVersion() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}