using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Managers;

public partial class AuthorityManager
{
    public async Task<AuthorityResult<AuthorityRole>> CreateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        role.Version = GenerateVersion();
        var createResult = await RoleRepository.CreateRoleAsync(role, cancellationToken);
        return AuthorityResult<AuthorityRole>.FromResult(createResult);
    }

    public async Task<Result> DeleteRolesAsync(List<AuthorityRole> roles, CancellationToken cancellationToken = default)
    {
        var result = await RoleRepository.DeleteRolesAsync(roles, cancellationToken);
        return result;
    }

    public async Task<Result<AuthorityRole>> UpdateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        var result = await RoleRepository.UpdateRoleAsync(role, cancellationToken);
        return result;
    }

    public async Task<ListResult<AuthorityRoleItem>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
    {
        var searchResult = await RoleRepository.GetRolesAsync(limit, offset, search, cancellationToken);
        return searchResult;
    }

    public async Task AddRolesToUserAsync(List<AuthorityRole> roles, AuthorityUser user, CancellationToken cancellationToken = default)
    {
        var usrValidation = await IsValidUserAsync(user, cancellationToken);
        if (!usrValidation.Success)
        {
            _logger.Error(usrValidation.Exception ?? new Exception("Validation for user failed!"), "Invalid user!");
            return;
        }
        
        var checkResult = await RoleRepository.HasRolesAsync(user.Id, roles, cancellationToken);
        if (!checkResult.Success)
        {
            return;
        }

        var rolesToAdd = roles;
        if (checkResult.Count != 0)
        {
            rolesToAdd = roles.Where(r => !checkResult.Items.Contains(r.Id)).ToList();
        }

        var addResult = await RoleRepository.AddRolesLinkAsync(rolesToAdd, user.Id, cancellationToken);
        if (!addResult.Success)
        {
            _logger.Error(addResult.Exception ?? new Exception("Adding role to user failed!, No further information available!"),"Failed to add role to user!");
        }
    }

    public async Task RemoveRolesFromUserAsync(List<AuthorityRole> roles, AuthorityUser user, CancellationToken cancellationToken = default)
    {
        var usrValidation = await IsValidUserAsync(user, cancellationToken);
        if (!usrValidation.Success)
        {
            _logger.Error(usrValidation.Exception ?? new Exception("Validation for user failed!"), "Invalid user!");
            return;
        }
        
        var checkResult = await RoleRepository.HasRolesAsync(user.Id, roles, cancellationToken);
        if (!checkResult.Success)
        {
            return;
        }

        var rolesToRemove = roles;
        if (checkResult.Count != 0)
        {
            rolesToRemove = roles.Where(r => !checkResult.Items.Contains(r.Id)).ToList();
        }
        
        var removeResult = await RoleRepository.DeleteRolesLinkAsync(rolesToRemove, user.Id, cancellationToken);
        if (!removeResult.Success)
        {
            _logger.Error(removeResult.Exception ?? new Exception("Removing roles from user failed!"), "Failed to remove roles from user!");
        }
    }

    public async Task AddRolesToGroupAsync(List<AuthorityRole> roles, AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        var checkResult = await RoleRepository.HasRolesAsync(group.Id, roles, cancellationToken);
        if (!checkResult.Success)
        {
            return;
        }
        
        var rolesToAdd = roles;
        if (checkResult.Count != 0)
        {
            rolesToAdd = roles.Where(r => !checkResult.Items.Contains(r.Id)).ToList();
        }
        
        var addResult = await RoleRepository.AddRolesLinkAsync(rolesToAdd, group.Id, cancellationToken);
        if (!addResult.Success)
        {
            _logger.Error(addResult.Exception ?? new Exception("Adding roles to group failed!"), "Failed to add roles to group!");
        }
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
        
        var searchIds = new List<Guid> { user.Id };
        
        var usrGroups = await GetUserGroupsAsync(user, cancellationToken);
        if (usrGroups.Success)
        {
            searchIds.AddRange(usrGroups.Items.Select(g => g.Id).ToList());
        }
        
        return await RoleRepository.GetLinkedRolesAsync(searchIds, cancellationToken);
    }

    public async Task<ListResult<AuthorityRole>> GetGroupRolesAsync(List<Guid> groupIds, CancellationToken cancellationToken = default)
    {
        return await RoleRepository.GetLinkedRolesAsync(groupIds, cancellationToken);
    }
}