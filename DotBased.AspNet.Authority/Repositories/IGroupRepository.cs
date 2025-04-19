using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IGroupRepository
{
    public Task<QueryItems<AuthorityGroupItem>> GetGroupsAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<AuthorityGroup?> GetGroupByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<bool> AddUsersToGroupAsync(List<AuthorityUser> users, AuthorityGroup group, CancellationToken cancellationToken = default);
    public Task<List<AuthorityGroup>> GetUserGroupsAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<AuthorityGroup?> CreateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default);
    public Task<AuthorityGroup?> UpdateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default);
    public Task<bool> DeleteGroupsAsync(List<AuthorityGroup> groups, CancellationToken cancellationToken = default);
}