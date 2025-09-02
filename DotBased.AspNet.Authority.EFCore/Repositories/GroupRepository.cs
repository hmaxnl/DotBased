using DotBased.AspNet.Authority.EFCore.Models;
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

    public async Task<bool> AddUsersToGroupAsync(List<AuthorityUser> users, AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        if (!context.Groups.Any(g => g.Id == group.Id))
        {
            return false;
        }
        
        var usersToAdd = users.Where(u => !context.UserGroups.Any(ug => ug.UserId == u.Id)).ToList();
        if (usersToAdd.Count == 0)
        {
            return false;
        }

        foreach (var user in usersToAdd)
        {
            context.UserGroups.Add(new UserGroups() { UserId = user.Id, GroupId = group.Id });
        }
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult > 0;
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

    public async Task<bool> DeleteGroupsAsync(List<AuthorityGroup> groups, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var groupIds = groups.Select(g => g.Id).ToList();
        
        context.Groups.RemoveRange(groups);
        context.UserGroups.RemoveRange(context.UserGroups.Where(ug => groupIds.Contains(ug.GroupId)));
        context.RoleLinks.RemoveRange(context.RoleLinks.Where(rl => groupIds.Contains(rl.LinkId)));
        
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0;
    }
}