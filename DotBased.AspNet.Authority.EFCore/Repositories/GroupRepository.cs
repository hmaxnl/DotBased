using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class GroupRepository : IGroupRepository
{
    public Task<ListResult<AuthorityGroupItem>> GetGroupsAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityAttribute>> GetGroupByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityAttribute>> CreateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityAttribute>> UpdateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}