using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class GroupRepository(IDbContextFactory<AuthorityContext> contextFactory) : RepositoryBase, IGroupRepository
{
    public async Task<ListResult<AuthorityGroupItem>> GetGroupsAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
    {
        try
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
            return ListResult<AuthorityGroupItem>.Ok(select, total, limit, offset);
        }
        catch (Exception e)
        {
            return HandleExceptionListResult<AuthorityGroupItem>("Failed to get Groups", e);
        }
    }

    public async Task<Result<AuthorityGroup>> GetGroupByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            if (!Guid.TryParse(id, out var groupId))
            {
                return Result<AuthorityGroup>.Failed("Invalid group id");
            }
            var group = await context.Groups.Where(g => g.Id == groupId).Include(g => g.Attributes).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            return Result<AuthorityGroup>.HandleResult(group, "Group not found");
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityGroup>("Failed to get Group", e);
        }
    }

    public async Task<ListResult<AuthorityGroup>> GetUserGroupsAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var userJoinGroups = context.UserGroups.Where(ug => ug.UserId == user.Id).Select(ug => ug.GroupId);
            var userGroups = context.Groups.Where(g => userJoinGroups.Contains(g.Id));
            return ListResult<AuthorityGroup>.Ok(userGroups, userGroups.Count());
        }
        catch (Exception e)
        {
            return HandleExceptionListResult<AuthorityGroup>("Failed to get Groups", e);
        }
    }

    public async Task<Result<AuthorityGroup>> CreateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            if (group.Id == Guid.Empty)
            {
                return Result<AuthorityGroup>.Failed("Id cannot be empty.");
            }
            var entry = context.Groups.Add(group);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result<AuthorityGroup>.Failed("Failed to create group.") : Result<AuthorityGroup>.Ok(entry.Entity);
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityGroup>("Failed to create group!", e);
        }
    }

    public async Task<Result<AuthorityGroup>> UpdateGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var currentGroup = await context.Groups.FirstOrDefaultAsync(g => g.Id == group.Id ,cancellationToken);
            if (currentGroup == null)
            {
                return Result<AuthorityGroup>.Failed("Group not found.");
            }

            if (currentGroup.Version != group.Version)
            {
                return Result<AuthorityGroup>.Failed("Group version does not match, version validation failed!");
            }
            
            var entry = context.Groups.Update(group);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result<AuthorityGroup>.Failed("Failed to update group.") : Result<AuthorityGroup>.Ok(entry.Entity);
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityGroup>("Failed to update group!", e);
        }
    }

    public async Task<Result> DeleteGroupAsync(AuthorityGroup group, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var currentGroup = await context.Groups.FirstOrDefaultAsync(g => g.Id == group.Id, cancellationToken);
            if (currentGroup == null)
            {
                return Result.Failed("Group not found, cannot delete group!");
            }
            context.Groups.Remove(currentGroup);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result.Failed("Failed to delete group.") : Result.Ok();
        }
        catch (Exception e)
        {
            return HandleException("Failed to delete group!", e);
        }
    }
}