using DotBased.AspNet.Authority.EFCore.Models;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore.Repositories;


public class RoleRepository(IDbContextFactory<AuthorityContext> contextFactory) : RepositoryBase, IRoleRepository
{
    public async Task<ListResult<AuthorityRoleItem>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
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
            return ListResult<AuthorityRoleItem>.Ok(select, total, limit, offset);
        }
        catch (Exception e)
        {
            return HandleExceptionListResult<AuthorityRoleItem>("Failed to get roles.", e);
        }
    }

    public async Task<Result<AuthorityRole>> GetRoleByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            if (!Guid.TryParse(id, out var guid))
            {
                return Result<AuthorityRole>.Failed("Invalid id!");
            }
            var role = await context.Roles.Where(r => r.Id == guid).Include(r => r.Attributes).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            return Result<AuthorityRole>.HandleResult(role, "Role not found!");
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityRole>("Failed to get role!", e);
        }
    }

    public async Task<Result<AuthorityRole>> CreateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            if (role.Id == Guid.Empty)
            {
                return Result<AuthorityRole>.Failed("Id cannot be empty!");
            }
            var entity = context.Roles.Add(role);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result<AuthorityRole>.Failed("Failed to create role!") : Result<AuthorityRole>.Ok(entity.Entity);
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityRole>("Failed to create role!", e);
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
                return Result<AuthorityRole>.Failed("Role not found!");
            }

            if (role.Version != currentRole.Version)
            {
                return Result<AuthorityRole>.Failed("Role version does not match, version validation failed!");
            }
            
            var entity = context.Roles.Update(role);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result<AuthorityRole>.Failed("Failed to update role!") : Result<AuthorityRole>.Ok(entity.Entity);
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityRole>("Failed to update role!", e);
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
            
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result.Failed("Failed to delete roles!") : Result.Ok();
        }
        catch (Exception e)
        {
            return HandleException("Failed to delete role!", e);
        }
    }

    public async Task<ListResult<AuthorityRoleItem>> GetUserRolesAsync(AuthorityUser user, int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
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
            return ListResult<AuthorityRoleItem>.Ok(roles, limit, offset);
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
            return saveResult <= 0 ? Result.Failed("Failed to ad role link!") : Result.Ok();
        }
        catch (Exception e)
        {
            return HandleException("Failed to add role link!", e);
        }
    }

    public async Task<ListResult<AuthorityRole>> GetLinkedRolesAsync(List<Guid> linkIds, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var linkedRoles = context.RoleLinks.Where(r => linkIds.Contains(r.LinkId)).Select(r => r.RoleId);
            var roleList = await context.Roles.Where(r => linkedRoles.Contains(r.Id)).ToListAsync(cancellationToken);
            return ListResult<AuthorityRole>.Ok(roleList.DistinctBy(r => r.Id));
        }
        catch (Exception e)
        {
            return HandleExceptionListResult<AuthorityRole>("Failed to get linked roles!", e);
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
            return saveResult <= 0 ? Result.Failed("Failed to delete role links!") : Result.Ok();
        }
        catch (Exception e)
        {
            return HandleException("Failed to delete role link!", e);
        }
    }

    public async Task<ListResult<Guid>> HasRolesAsync(Guid linkId, List<AuthorityRole> roles, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var hasRoles = await context.RoleLinks.Where(r => r.LinkId == linkId && roles.Any(ar => ar.Id == r.RoleId)).Select(r => r.RoleId).ToListAsync(cancellationToken);
            return ListResult<Guid>.Ok(hasRoles);
        }
        catch (Exception e)
        {
            return HandleExceptionListResult<Guid>("Failed to determine role for user!", e);
        }
    }
}