using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Managers;

public partial class AuthorityManager
{
    public async Task<Result<AuthorityRole>> CreateRoleAsync(AuthorityRole role, CancellationToken? cancellationToken = null)
    {
        return Result<AuthorityRole>.Failed("Not implemented!");
    }

    public async Task<Result> DeleteRoleAsync(AuthorityRole role, CancellationToken? cancellationToken = null)
    {
        return Result.Failed("Not implemented!");
    }

    public async Task<Result<AuthorityRole>> UpdateRoleAsync(AuthorityRole role, CancellationToken? cancellationToken = null)
    {
        return Result<AuthorityRole>.Failed("Not implemented!");
    }

    public async Task AddUserToRole(AuthorityUser user, AuthorityRole role, CancellationToken? cancellationToken = null)
    {
        
    }
}