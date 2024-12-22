namespace DotBased.AspNet.Authority.Interfaces;

public interface ISecurityVersionRepository<in TRepositoryObject>
{
    public Task<long> GetSecurityVersionAsync(TRepositoryObject obj);
    
}