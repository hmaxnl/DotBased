using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IRoleRepository
{
    public Task<QueryItems<AuthorityRoleItem>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<AuthorityRole?> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<AuthorityRole?> CreateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default);
    public Task<AuthorityRole?> UpdateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default);
    public Task<bool> DeleteRolesAsync(List<AuthorityRole> roles, CancellationToken cancellationToken = default);
    public Task<bool> AddRolesLinkAsync(List<AuthorityRole> roles, Guid linkId, CancellationToken cancellationToken = default);
    public Task<List<AuthorityRole>> GetLinkedRolesAsync(List<Guid> linkIds, CancellationToken cancellationToken = default);
    public Task<bool> UnlinkRolesAsync(List<AuthorityRole> roles, Guid linkId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Return the role ids the linkId has.
    /// </summary>
    /// <param name="linkId"></param>
    /// <param name="roles"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<List<Guid>> GetRolesFromLinkAsync(Guid linkId, List<AuthorityRole> roles, CancellationToken cancellationToken = default);
}