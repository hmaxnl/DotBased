namespace DotBased.ASP.Auth;

public interface ISessionStateProvider
{
    public const string SessionStateName = "BasedServerSession";
    public Task<Result<string>> GetSessionStateAsync();
    public Task<Result> SetSessionStateAsync(string state);
}