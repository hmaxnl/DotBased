using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class RoleRepository : IRoleRepository
{
    public Task<ListResult<AuthorityRole>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}