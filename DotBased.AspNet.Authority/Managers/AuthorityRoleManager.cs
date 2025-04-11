using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.Monads;

namespace DotBased.AspNet.Authority.Managers;

public partial class AuthorityManager
{
    public async Task<Result<AuthorityRole>> CreateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        role.Version = GenerateVersion();
        var createResult = await RoleRepository.CreateRoleAsync(role, cancellationToken);
        if (createResult == null)
        {
            return ResultError.Fail("Failed to create new role.");
        }

        return createResult;
    }

    public async Task<Result> DeleteRolesAsync(List<AuthorityRole> roles, CancellationToken cancellationToken = default)
    {
        var success = await RoleRepository.DeleteRolesAsync(roles, cancellationToken);
        return success ? Result.Success() : ResultError.Fail("Failed to delete roles.");
    }

    public async Task<Result<AuthorityRole>> UpdateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        var result = await RoleRepository.UpdateRoleAsync(role, cancellationToken);
        if (result == null)
        {
            return ResultError.Fail("Failed to update role.");
        }

        return result;
    }

    public async Task<Result<QueryItems<AuthorityRoleItem>>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
    {
        var searchResult = await RoleRepository.GetRolesAsync(limit, offset, search, cancellationToken);
        return searchResult;
    }

    public async Task<Result> AddRolesToUserAsync(List<AuthorityRole> roles, AuthorityUser user, CancellationToken cancellationToken = default)
    {
        var usrValidation = await IsValidUserAsync(user, cancellationToken);
        if (!usrValidation.Success)
        {
            return ResultError.Fail("Validation for user failed!");
        }
        
        var linkedRoles = await RoleRepository.GetRolesFromLinkAsync(user.Id, roles, cancellationToken);

        var rolesToAdd = roles;
        if (linkedRoles.Count != 0)
        {
            rolesToAdd = roles.Where(r => !linkedRoles.Contains(r.Id)).ToList();
        }

        var addSuccess = await RoleRepository.AddRolesLinkAsync(rolesToAdd, user.Id, cancellationToken);
        
        return addSuccess ? Result.Success() : ResultError.Fail("Failed to add roles.");
    }

    public async Task<Result> RemoveRolesFromUserAsync(List<AuthorityRole> roles, AuthorityUser user, CancellationToken cancellationToken = default)
    {
        var usrValidation = await IsValidUserAsync(user, cancellationToken);
        if (!usrValidation.Success)
        {
            return ResultError.Fail("Validation for user failed!");
        }
        
        var linkedRoles = await RoleRepository.GetRolesFromLinkAsync(user.Id, roles, cancellationToken);

        var rolesToRemove = roles;
        if (linkedRoles.Count != 0)
        {
            rolesToRemove = roles.Where(r => !linkedRoles.Contains(r.Id)).ToList();
        }
        
        var removeResult = await RoleRepository.UnlinkRolesAsync(rolesToRemove, user.Id, cancellationToken);
        return removeResult ? Result.Success() : ResultError.Fail("Failed to remove roles.");
    }

    public async Task<Result> AddRolesToGroupAsync(List<AuthorityRole> roles, AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        var linkedRoles = await RoleRepository.GetRolesFromLinkAsync(group.Id, roles, cancellationToken);
        
        var rolesToAdd = roles;
        if (linkedRoles.Count != 0)
        {
            rolesToAdd = roles.Where(r => !linkedRoles.Contains(r.Id)).ToList();
        }
        
        var linkResult = await RoleRepository.AddRolesLinkAsync(rolesToAdd, group.Id, cancellationToken);
        return linkResult ? Result.Success() : ResultError.Fail("Failed to add roles.");
    }

    /// <summary>
    /// Get all roles (including group roles) that the user has.
    /// </summary>
    /// <param name="user">The user to get the roles from</param>
    /// <param name="cancellationToken"></param>
    public async Task<Result<List<AuthorityRole>>> GetUserRolesAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            var usrValidation = await IsValidUserAsync(user, cancellationToken);
            if (!usrValidation.Success)
            {
                return ResultError.Fail("Invalid user");
            }

            var searchIds = new List<Guid> { user.Id };

            var usrGroups = await GetUserGroupsAsync(user, cancellationToken);
            if (usrGroups.Success)
            {
                searchIds.AddRange(usrGroups.Items.Select(g => g.Id).ToList());
            }

            var linkedRolesResult = await RoleRepository.GetLinkedRolesAsync(searchIds, cancellationToken);
            return linkedRolesResult;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<List<AuthorityRole>>> GetGroupRolesAsync(List<Guid> groupIds, CancellationToken cancellationToken = default)
    {
        try
        {
            var linkedRolesResult = await RoleRepository.GetLinkedRolesAsync(groupIds, cancellationToken);
            return linkedRolesResult;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}