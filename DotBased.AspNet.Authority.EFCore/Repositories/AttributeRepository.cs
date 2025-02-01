using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class AttributeRepository : IAttributeRepository
{
    public Task<ListResult<AuthorityAttributeItem>> GetAttributesAsync(int limit = 20, int offset = 0, string search = "",
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityAttribute>> GetAttributeByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityAttribute>> CreateAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityAttribute>> UpdateAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}