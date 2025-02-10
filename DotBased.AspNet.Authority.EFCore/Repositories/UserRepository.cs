using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class UserRepository(IDbContextFactory<AuthorityContext> contextFactory) : IUserRepository
{
    public async Task<ListResult<AuthorityUserItem>> GetAuthorityUsersAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var query = context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    $"{u.Id} {u.Name} {u.UserName} {u.EmailAddress} {u.PhoneNumber}".Contains(search,
                        StringComparison.CurrentCultureIgnoreCase));
            }

            var totalCount = query.Count();
            var selected = query.Skip(offset).Take(limit).Select(u => new AuthorityUserItem()
            {
                Id = u.Id,
                UserName = u.UserName,
                EmailAddress = u.EmailAddress,
                PhoneNumber = u.PhoneNumber
            });
            return ListResult<AuthorityUserItem>.Ok(selected, totalCount, limit, offset);
        }
        catch (Exception e)
        {
            return ListResult<AuthorityUserItem>.Failed("Failed to get users.", e);
        }
    }

    public async Task<Result<AuthorityUser>> GetAuthorityUserByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            if (!Guid.TryParse(id, out var guid))
            {
                return Result<AuthorityUser>.Failed("Invalid id!");
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == guid, cancellationToken: cancellationToken);
            return Result<AuthorityUser>.HandleResult(user, "User not found.");
        }
        catch (Exception e)
        {
            return Result<AuthorityUser>.Failed("Failed to get user.", e);
        }
    }

    public async Task<Result<AuthorityUser>> CreateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            if (user.Id == Guid.Empty)
            {
                return Result<AuthorityUser>.Failed("Id cannot be empty!");
            }
            var entity = context.Users.Add(user);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result<AuthorityUser>.Failed("Failed to create user!") : Result<AuthorityUser>.Ok(entity.Entity);
        }
        catch (Exception e)
        {
            return Result<AuthorityUser>.Failed("Failed to create user.", e);
        }
    }

    public async Task<Result<AuthorityUser>> UpdateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (usr == null)
            {
                return Result<AuthorityUser>.Failed("User not found!");
            }

            if (usr.Version != user.Version || usr.SecurityVersion != user.SecurityVersion)
            {
                return Result<AuthorityUser>.Failed("Version validation failed!");
            }
            
            var entity = context.Users.Update(user);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result<AuthorityUser>.Failed("Failed to save updated user!") : Result<AuthorityUser>.Ok(entity.Entity);
        }
        catch (Exception e)
        {
            return Result<AuthorityUser>.Failed("Failed to update user!", e);
        }
    }

    public async Task<Result> DeleteUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (usr == null)
            {
                return Result.Failed("User not found!");
            }
            context.Users.Remove(usr);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result.Failed("Failed to delete user!") : Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Failed("Failed to delete user!", e);
        }
    }

    public Task<Result<AuthorityUser>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SetVersionAsync(AuthorityUser user, long version, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<long>> GetVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SetSecurityVersionAsync(AuthorityUser user, long version, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<long>> GetSecurityVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}