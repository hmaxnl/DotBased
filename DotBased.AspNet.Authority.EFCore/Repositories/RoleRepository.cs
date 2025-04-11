using DotBased.AspNet.Authority.EFCore.Models;
using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotBased.AspNet.Authority.EFCore.Repositories;


public class RoleRepository(IDbContextFactory<AuthorityContext> contextFactory, ILogger<RoleRepository> logger) : RepositoryBase, IRoleRepository
{
    public async Task<QueryItems<AuthorityRoleItem>> GetRolesAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
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

    public async Task<AuthorityRole?> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var role = await context.Roles.Where(r => r.Id == id).Include(r => r.Attributes).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return role;
    }

    public async Task<AuthorityRole?> CreateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        if (role.Id == Guid.Empty)
        {
            throw new Exception("Role id is required!");
        }
        var entity = context.Roles.Add(role);
        var saveResult = await context.SaveChangesAsync(cancellationToken);

        return saveResult != 0 ? entity.Entity : null;
    }

    public async Task<AuthorityRole?> UpdateRoleAsync(AuthorityRole role, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var currentRole = await context.Roles.FirstOrDefaultAsync(r => r.Id == role.Id, cancellationToken: cancellationToken);
        if (currentRole == null)
        {
            throw new Exception($"Role with id {role.Id} not found!");
        }

        if (role.Version != currentRole.Version)
        {
            throw new Exception("Role version does not match!");
        }
            
        var entity = context.Roles.Update(role);
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0 ? entity.Entity : null;
    }

    public async Task<bool> DeleteRolesAsync(List<AuthorityRole> roles, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var roleIds = roles.Select(r => r.Id).ToList();
        
        context.Roles.RemoveRange(roles);
        context.RoleLinks.RemoveRange(context.RoleLinks.Where(rl => roleIds.Contains(rl.RoleId)));
        
        var removedRoles = await context.SaveChangesAsync(cancellationToken);
        if (removedRoles == roles.Count)
        {
            return true;
        }
        logger.LogError("Failed to remove all roles, {removedRoles}/{totalRoles} roles removed!", removedRoles, roles.Count);
        return false;
    }

    public async Task<bool> AddRolesLinkAsync(List<AuthorityRole> roles, Guid linkId, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        foreach (var role in roles)
        {
            context.RoleLinks.Add(new RoleLink { LinkId = linkId, RoleId = role.Id });
        }
        var linkedRoles = await context.SaveChangesAsync(cancellationToken);
        if (linkedRoles == roles.Count)
        {
            return true;
        }

        logger.LogError("Failed to link all given roles, {linkedRoles}/{totalRoles} roles linked!", linkedRoles, roles.Count);
        return false;
    }

    public async Task<List<AuthorityRole>> GetLinkedRolesAsync(List<Guid> linkIds, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var linkedRoles = context.RoleLinks.Where(r => linkIds.Contains(r.LinkId)).Select(r => r.RoleId);
        var roleList = await context.Roles.Where(r => linkedRoles.Contains(r.Id)).ToListAsync(cancellationToken);
        return roleList.DistinctBy(r => r.Id).ToList();
    }

    public async Task<bool> UnlinkRolesAsync(List<AuthorityRole> roles, Guid linkId, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var roleIds = roles.Select(r => r.Id).ToList();
        context.RoleLinks.RemoveRange(context.RoleLinks.Where(rg => rg.LinkId == linkId && roleIds.Contains(rg.RoleId)));
        var unlinkedRoles = await context.SaveChangesAsync(cancellationToken);
        if (unlinkedRoles == roles.Count)
        {
            return true;
        }

        logger.LogError("Failed to remove all linked roles, {unlinkedRoles}/{totalRoles} roles unlinked!", unlinkedRoles, roles.Count);
        return false;
    }

    public async Task<List<Guid>> GetRolesFromLinkAsync(Guid linkId, List<AuthorityRole> roles, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.RoleLinks.Where(r => r.LinkId == linkId && roles.Any(ar => ar.Id == r.RoleId)).Select(r => r.RoleId).ToListAsync(cancellationToken);
    }
}