namespace DotBased.AspNet.Authority.Repositories;

public interface IAuthorityRepository
{
    public Task<long> GetVersion();
    public Task SetVersion(long version);
}