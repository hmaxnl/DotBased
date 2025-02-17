using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IGroupRepository
{
    public Task<ListResult<AuthorityGroupItem>> GetGroupsAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<Result<AuthorityGroup>> GetGroupByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityGroup>> CreateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityGroup>> UpdateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default);
    public Task<Result> DeleteGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default);
}