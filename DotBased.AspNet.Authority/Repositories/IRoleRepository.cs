using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IRoleRepository
{
    public Task<ListResult<AuthorityRoleItem>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<Result<AuthorityRole>> GetRoleByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityRole>> CreateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityRole>> UpdateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default);
    public Task<Result> DeleteRolesAsync(List<AuthorityRole> roles, CancellationToken cancellationToken = default);
    public Task<Result> AddRolesLinkAsync(List<AuthorityRole> roles, Guid linkId, CancellationToken cancellationToken = default);
    public Task<ListResult<AuthorityRole>> GetLinkedRolesAsync(List<Guid> linkIds, CancellationToken cancellationToken = default);
    public Task<Result> DeleteRolesLinkAsync(List<AuthorityRole> roles, Guid linkId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Return the role ids the linkId has.
    /// </summary>
    /// <param name="linkId"></param>
    /// <param name="roles"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ListResult<Guid>> HasRolesAsync(Guid linkId, List<AuthorityRole> roles, CancellationToken cancellationToken = default);
}