using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class GroupRepository(IDbContextFactory<AuthorityContext> contextFactory, ILogger<GroupRepository> logger) : RepositoryBase, IGroupRepository
{
    public async Task<QueryItems<AuthorityGroupItem>> GetGroupsAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var query = context.Groups.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(g => $"{g.Name} {g.Id}".Contains(search));
        }
        var total = await query.CountAsync(cancellationToken);
        var select = await query.OrderBy(g => g.Name).Skip(offset).Take(limit).Select(g => new AuthorityGroupItem()
        {
            Id = g.Id,
            Name = g.Name
        }).ToListAsync(cancellationToken);
        return QueryItems<AuthorityGroupItem>.Create(select, total, limit, offset);
    }

    public async Task<AuthorityGroup?> GetGroupByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        
        if (!Guid.TryParse(id, out var groupId))
        {
            throw new Exception($"Invalid group id: {id}");
        }
        
        return await context.Groups.Where(g => g.Id == groupId).Include(g => g.Attributes).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<AuthorityGroup>> GetUserGroupsAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var userJoinGroups = context.UserGroups.Where(ug => ug.UserId == user.Id).Select(ug => ug.GroupId);
        var userGroups = context.Groups.Where(g => userJoinGroups.Contains(g.Id));
        return userGroups.ToList();
    }

    public async Task<AuthorityGroup?> CreateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        if (group.Id == Guid.Empty)
        {
            throw new Exception($"Invalid group id: {group.Id}");
        }
        var entry = context.Groups.Add(group);
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0 ? entry.Entity : null;
    }

    public async Task<AuthorityGroup?> UpdateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var currentGroup = await context.Groups.FirstOrDefaultAsync(g => g.Id == group.Id ,cancellationToken);
        if (currentGroup == null)
        {
            logger.LogError("Group with id {groupId} not found.", group.Id);
            return null;
        }

        if (currentGroup.Version != group.Version)
        {
            logger.LogError("Group version validation failed.");
            return null;
        }
            
        var entry = context.Groups.Update(group);
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0 ? entry.Entity : null;
    }

    public async Task<bool> DeleteGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var currentGroup = await context.Groups.FirstOrDefaultAsync(g => g.Id == group.Id, cancellationToken);
        if (currentGroup == null)
        {
            logger.LogError("Group with id {groupId} not found.", group.Id);
            return false;
        }
        context.Groups.Remove(currentGroup);
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0;
    }
}