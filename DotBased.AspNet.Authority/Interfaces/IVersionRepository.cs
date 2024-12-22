namespace DotBased.AspNet.Authority.Interfaces;

public interface IVersionRepository<in TRepositoryObject>
{
    public Task<long> GetVersionAsync(TRepositoryObject obj);
}