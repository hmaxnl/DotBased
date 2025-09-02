using DotBased.AspNet.Authority.Models.Authority;
using DotBased.Monads;

namespace DotBased.AspNet.Authority.Managers;

public partial class AuthorityManager
{
    public async Task<Result<List<AuthorityGroup>>> GetUserGroupsAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            return await GroupRepository.GetUserGroupsAsync(user, cancellationToken);
        }
        catch (Exception e)
        {
            return e;
        }
    }
}