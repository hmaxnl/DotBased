namespace DotBased.ASP.Auth;

public interface ISessionStateProvider
{
    public const string SessionStateName = "BasedServerSession";
    public Task<ResultOld<string>> GetSessionStateAsync();
    public Task<ResultOld> SetSessionStateAsync(string state);
}