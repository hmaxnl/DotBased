using DotBased.AspNet.Authority.EFCore.Models;
using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using DotBased.Monads;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore.Repositories;


public class RoleRepository(IDbContextFactory<AuthorityContext> contextFactory) : RepositoryBase, IRoleRepository
{
    public async Task<Result<QueryItems<AuthorityRoleItem>>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var query = context.Roles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    $"{r.Name} {r.Id}".Contains(search, StringComparison.CurrentCultureIgnoreCase));
            }

            var total = await query.CountAsync(cancellationToken);
            var select = await query.OrderBy(r => r.Name).Skip(offset).Take(limit).Select(r => new AuthorityRoleItem()
            {
                Id = r.Id,
                Name = r.Name
            }).ToListAsync(cancellationToken: cancellationToken);
            return QueryItems<AuthorityRoleItem>.Create(select, total, limit, offset);
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<AuthorityRole>> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var role = await context.Roles.Where(r => r.Id == id).Include(r => r.Attributes).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (role != null)
            {
                return role;
            }
            return ResultError.Fail("Role not found!");
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<AuthorityRole>> CreateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            if (role.Id == Guid.Empty)
            {
                return ResultError.Fail("Id cannot be empty!");
            }
            var entity = context.Roles.Add(role);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? ResultError.Fail("Failed to create role!") : entity.Entity;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<AuthorityRole>> UpdateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var currentRole = await context.Roles.FirstOrDefaultAsync(r => r.Id == role.Id, cancellationToken: cancellationToken);
            if (currentRole == null)
            {
                return ResultError.Fail("Role not found!");
            }

            if (role.Version != currentRole.Version)
            {
                return ResultError.Fail("Role version does not match, version validation failed!");
            }
            
            var entity = context.Roles.Update(role);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? ResultError.Fail("Failed to update role!") : entity.Entity;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result> DeleteRolesAsync(List<AuthorityRole> roles, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var roleIds = roles.Select(r => r.Id).ToList();
            
            context.Roles.RemoveRange(roles);
            context.RoleLinks.RemoveRange(context.RoleLinks.Where(rg => roleIds.Contains(rg.RoleId)));
            
            var removeResult = await context.SaveChangesAsync(cancellationToken);
            return removeResult == roles.Count? Result.Success() : ResultError.Fail($"Not all roles have been removed! {removeResult} of {roles.Count} roles removed!");
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<ListResultOld<AuthorityRoleItem>> GetUserRolesAsync(AuthorityUser user, int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var roleIds = await context.RoleLinks.Where(r => r.LinkId == user.Id).Select(i => i.RoleId).ToListAsync(cancellationToken: cancellationToken);
            var rolesQuery = context.Roles.Where(r => roleIds.Contains(r.Id));
            if (!string.IsNullOrEmpty(search))
            {
                rolesQuery = rolesQuery.Where(r => r.Name.Contains(search));
            }

            var roles = rolesQuery.Where(r => roleIds.Contains(r.Id)).Skip(offset).Take(limit).Select(r => new AuthorityRoleItem()
            {
                Id = r.Id,
                Name = r.Name
            });
            return ListResultOld<AuthorityRoleItem>.Ok(roles, limit, offset);
        }
        catch (Exception e)
        {
            return HandleExceptionListResult<AuthorityRoleItem>("Failed to get user roles.", e);
        }
    }

    public async Task<Result> AddRolesLinkAsync(List<AuthorityRole> roles, Guid linkId, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            foreach (var role in roles)
            {
                context.RoleLinks.Add(new RoleLink() { LinkId = linkId, RoleId = role.Id });
            }
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult == roles.Count ? Result.Success() : ResultError.Fail($"Not all roles have been linked! {saveResult} of {roles.Count} roles linked!");
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<List<AuthorityRole>>> GetLinkedRolesAsync(List<Guid> linkIds, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var linkedRoles = context.RoleLinks.Where(r => linkIds.Contains(r.LinkId)).Select(r => r.RoleId);
            var roleList = await context.Roles.Where(r => linkedRoles.Contains(r.Id)).ToListAsync(cancellationToken);
            return roleList.DistinctBy(r => r.Id).ToList();
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result> DeleteRolesLinkAsync(List<AuthorityRole> roles, Guid linkId, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var roleIds = roles.Select(r => r.Id).ToList();
            context.RoleLinks.RemoveRange(context.RoleLinks.Where(rg => rg.LinkId == linkId && roleIds.Contains(rg.RoleId)));
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult == roles.Count ? Result.Success() : ResultError.Fail($"Not all roles have been unlinked! {saveResult} of {roles.Count} roles unlinked!");
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<List<Guid>>> HasRolesAsync(Guid linkId, List<AuthorityRole> roles, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var hasRoles = await context.RoleLinks.Where(r => r.LinkId == linkId && roles.Any(ar => ar.Id == r.RoleId)).Select(r => r.RoleId).ToListAsync(cancellationToken);
            return hasRoles;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}