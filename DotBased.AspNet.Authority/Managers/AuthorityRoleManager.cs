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

    public async Task<ListResult<AuthorityRole>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken? cancellationToken = null)
    {
        /*
         * Search by role name & id
         * Order by name, created date, creator? (paging)
         */
        return ListResult<AuthorityRole>.Failed("Not implemented!");
    }

    public async Task AddRoleToUserAsync(AuthorityUser user, AuthorityRole role, CancellationToken? cancellationToken = null)
    {
        /*
            - Validate User & Role
            - Check if role is already in linked to user (if user already has the role, return)
            - Add to UsersRoles table
        */
    }

    public async Task RemoveRoleFromUserAsync(AuthorityRole role, AuthorityUser user, CancellationToken? cancellationToken = null)
    {
    }

    public async Task AddRoleToGroupAsync(AuthorityRole role, AuthorityGroup group, CancellationToken? cancellationToken = null)
    {
    }

    /// <summary>
    /// Get all roles (including group roles) that the user has.
    /// </summary>
    /// <param name="user">The user to get the roles from</param>
    /// <param name="cancellationToken"></param>
    public async Task<ListResult<AuthorityRole>> GetUserRolesAsync(AuthorityUser user, CancellationToken? cancellationToken = null)
    {
        /*
         * - Validate user
         * - Get user groups (id)
         * - Get roles contained from user
         * - Get roles contained from groups (if any)
         * - Order by (for paging)
         */
        
        return ListResult<AuthorityRole>.Failed("Not implemented!");
    }
}