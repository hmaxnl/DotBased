using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class RoleRepository : IRoleRepository
{
    public Task<ListResult<AuthorityRoleItem>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityRole>> GetRoleByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityRole>> CreateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityRole>> UpdateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}