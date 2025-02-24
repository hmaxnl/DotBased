using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Managers;

public partial class AuthorityManager
{
    public async Task<AuthorityResult<AuthorityRole>> CreateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        role.Version = GenerateVersion();
        var createResult = await roleRepository.CreateRoleAsync(role, cancellationToken);
        return AuthorityResult<AuthorityRole>.FromResult(createResult);
    }

    public async Task<Result> DeleteRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        var result = await roleRepository.DeleteRoleAsync(role, cancellationToken);
        return result;
    }

    public async Task<Result<AuthorityRole>> UpdateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        var result = await roleRepository.UpdateRoleAsync(role, cancellationToken);
        return result;
    }

    public async Task<ListResult<AuthorityRoleItem>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
    {
        var searchResult = await roleRepository.GetRolesAsync(limit, offset, search, cancellationToken);
        return searchResult;
    }

    public async Task AddRoleToUserAsync(AuthorityUser user, AuthorityRole role, CancellationToken cancellationToken = default)
    {
        /*
            - Validate User & Role
            - Check if role is already in linked to user (if user already has the role, return)
            - Add to UsersRoles table
        */
    }

    public async Task RemoveRoleFromUserAsync(AuthorityRole role, AuthorityUser user, CancellationToken cancellationToken = default)
    {
    }

    public async Task AddRoleToGroupAsync(AuthorityRole role, AuthorityGroup group, CancellationToken cancellationToken = default)
    {
    }

    /// <summary>
    /// Get all roles (including group roles) that the user has.
    /// </summary>
    /// <param name="user">The user to get the roles from</param>
    /// <param name="cancellationToken"></param>
    public async Task<ListResult<AuthorityRole>> GetUserRolesAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        var usrValidation = await IsValidUserAsync(user, cancellationToken);
        if (!usrValidation.Success)
        {
            return ListResult<AuthorityRole>.Failed("Invalid user");
        }
        
        List<AuthorityRole> roles = [];
        var usrRoles = await GetUserRolesAsync(user, cancellationToken);
        if (usrRoles.Success)
        {
            roles.AddRange(usrRoles.Items);
        }
        var usrGroups = await GetUserGroupsAsync(user, cancellationToken);
        if (usrGroups.Success)
        {
            var groupRolesResult = await GetGroupRolesAsync(usrGroups.Items.Select(g => g.Id).ToList(), cancellationToken);
            if (groupRolesResult.Success)
            {
                roles.AddRange(groupRolesResult.Items);
            }
        }
        return ListResult<AuthorityRole>.Ok(roles, roles.Count);
    }

    public async Task<ListResult<AuthorityRole>> GetGroupRolesAsync(List<Guid> groupIds, CancellationToken cancellationToken = default)
    {
        return ListResult<AuthorityRole>.Failed("Not implemented!");
    }
}