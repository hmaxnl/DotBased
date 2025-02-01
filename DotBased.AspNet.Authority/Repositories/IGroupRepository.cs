using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IGroupRepository
{
    public Task<ListResult<AuthorityGroupItem>> GetGroupsAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<Result<AuthorityAttribute>> GetGroupByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityAttribute>> CreateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityAttribute>> UpdateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default);
    public Task<Result> DeleteGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default);
}