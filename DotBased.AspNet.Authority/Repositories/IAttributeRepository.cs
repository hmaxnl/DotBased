using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IAttributeRepository
{
    public Task<QueryItems<AuthorityAttributeItem>> GetAttributesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<AuthorityAttribute?> GetAttributeByKeyAsync(string id, CancellationToken cancellationToken = default);
    public Task<AuthorityAttribute?> CreateAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default);
    public Task<AuthorityAttribute?> UpdateAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default);
    public Task<bool> DeleteAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default);
}