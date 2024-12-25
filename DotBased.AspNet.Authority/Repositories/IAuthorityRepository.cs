namespace DotBased.AspNet.Authority.Repositories;

public interface IAuthorityRepository
{
    public Task<int> GetVersion();
    public Task SetVersion(int version);
}