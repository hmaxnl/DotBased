using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IAttributeRepository
{
    public Task<ListResultOld<AuthorityAttributeItem>> GetAttributesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<ResultOld<AuthorityAttribute>> GetAttributeByKeyAsync(string id, CancellationToken cancellationToken = default);
    public Task<ResultOld<AuthorityAttribute>> CreateAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default);
    public Task<ResultOld<AuthorityAttribute>> UpdateAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default);
    public Task<ResultOld> DeleteAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default);
}