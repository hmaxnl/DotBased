using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IGroupRepository
{
    public Task<ListResultOld<AuthorityGroupItem>> GetGroupsAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<ResultOld<AuthorityGroup>> GetGroupByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<ListResultOld<AuthorityGroup>> GetUserGroupsAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<ResultOld<AuthorityGroup>> CreateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default);
    public Task<ResultOld<AuthorityGroup>> UpdateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default);
    public Task<ResultOld> DeleteGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default);
}