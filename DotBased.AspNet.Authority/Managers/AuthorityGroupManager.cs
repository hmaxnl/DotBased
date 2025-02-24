using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Managers;

public partial class AuthorityManager
{
    public async Task<ListResult<AuthorityGroupItem>> GetUserGroupsAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        return ListResult<AuthorityGroupItem>.Failed("Not implemented!");
    }
}