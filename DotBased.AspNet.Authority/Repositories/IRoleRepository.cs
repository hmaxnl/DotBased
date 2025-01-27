using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IRoleRepository
{
    public Task<ListResult<AuthorityRole>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
}