using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Managers;

public partial class AuthorityManager
{
    public async Task<ListResultOld<AuthorityGroup>> GetUserGroupsAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        return await GroupRepository.GetUserGroupsAsync(user, cancellationToken);
    }
}