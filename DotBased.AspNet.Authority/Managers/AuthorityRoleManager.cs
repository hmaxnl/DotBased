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
        return createResult;
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

    public async Task<Result<QueryItems<AuthorityRoleItem>>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
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
        var hasRolesList = checkResult.Match<List<Guid>>(success: v => v, (_) => []);

        var rolesToAdd = roles;
        if (hasRolesList.Count != 0)
        {
            rolesToAdd = roles.Where(r => !hasRolesList.Contains(r.Id)).ToList();
        }

        var addResult = await RoleRepository.AddRolesLinkAsync(rolesToAdd, user.Id, cancellationToken);
        addResult.Match(() =>
        {
            _logger.Debug("Role links successfully added!");
        }, e =>
        {
            _logger.Error(e.Exception ?? new Exception("Match failed!"), e.Description);
        });
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
        var hasRolesList = checkResult.Match<List<Guid>>(success: v => v, (_) => []);

        var rolesToRemove = roles;
        if (hasRolesList.Count != 0)
        {
            rolesToRemove = roles.Where(r => !hasRolesList.Contains(r.Id)).ToList();
        }
        
        var removeResult = await RoleRepository.DeleteRolesLinkAsync(rolesToRemove, user.Id, cancellationToken);
        removeResult.Match(() =>
        {
            _logger.Debug("Removed roles from user!");
        }, e =>
        {
            _logger.Error(e.Exception ?? new Exception("Removing roles from user failed!"), e.Description);
        });
    }

    public async Task AddRolesToGroupAsync(List<AuthorityRole> roles, AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        var checkResult = await RoleRepository.HasRolesAsync(group.Id, roles, cancellationToken);
        var hasRolesList = checkResult.Match<List<Guid>>(success: v => v, (_) => []);
        
        var rolesToAdd = roles;
        if (hasRolesList.Count != 0)
        {
            rolesToAdd = roles.Where(r => !hasRolesList.Contains(r.Id)).ToList();
        }
        
        var addResult = await RoleRepository.AddRolesLinkAsync(rolesToAdd, group.Id, cancellationToken);
        addResult.Match(() =>
        {
            _logger.Debug("Added roles to group.");
        }, e =>
        {
            _logger.Error(e.Exception ?? new Exception("Adding roles to group failed!"), e.Description);
        });
    }

    /// <summary>
    /// Get all roles (including group roles) that the user has.
    /// </summary>
    /// <param name="user">The user to get the roles from</param>
    /// <param name="cancellationToken"></param>
    public async Task<Result<List<AuthorityRole>>> GetUserRolesAsync(AuthorityUser user, CancellationToken cancellationToken = default)
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
        return linkedRolesResult.Match<List<AuthorityRole>>(roles => roles, e =>
        {
            _logger.Error(e.Exception ?? new Exception("Failed to get user roles!"), e.Description);
            return [];
        });
    }

    public async Task<Result<List<AuthorityRole>>> GetGroupRolesAsync(List<Guid> groupIds, CancellationToken cancellationToken = default)
    {
        var linkedRolesResult = await RoleRepository.GetLinkedRolesAsync(groupIds, cancellationToken);
        return linkedRolesResult.Match<List<AuthorityRole>>(roles => roles, e =>
        {
            _logger.Error(e.Exception ?? new Exception("Failed to get group roles!"), e.Description);
            return [];
        });
    }
}