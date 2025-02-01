using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IRoleRepository
{
    public Task<ListResult<AuthorityRoleItem>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<Result<AuthorityRole>> GetRoleByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityRole>> CreateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityRole>> UpdateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default);
    public Task<Result> DeleteRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default);
}