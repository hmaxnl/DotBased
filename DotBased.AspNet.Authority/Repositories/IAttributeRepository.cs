using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IAttributeRepository
{
    public Task<ListResult<AuthorityAttributeItem>> GetAttributesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<Result<AuthorityAttribute>> GetAttributeByKeyAsync(string id, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityAttribute>> CreateAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityAttribute>> UpdateAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default);
    public Task<Result> DeleteAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default);
}