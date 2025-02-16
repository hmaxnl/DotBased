using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class UserRepository(IDbContextFactory<AuthorityContext> contextFactory) : RepositoryBase, IUserRepository
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
            var selected = await query.OrderBy(u => u.UserName).Skip(offset).Take(limit).Select(u => new AuthorityUserItem()
            {
                Id = u.Id,
                UserName = u.UserName,
                EmailAddress = u.EmailAddress,
                PhoneNumber = u.PhoneNumber
            }).ToListAsync(cancellationToken: cancellationToken);
            return ListResult<AuthorityUserItem>.Ok(selected, totalCount, limit, offset);
        }
        catch (Exception e)
        {
            return HandleExceptionListResult<AuthorityUserItem>("Failed to get users.", e);
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

            var user = await context.Users.Where(u => u.Id == guid).Include(u => u.Attributes).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            return Result<AuthorityUser>.HandleResult(user, "User not found.");
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityUser>("Failed to get user.", e);
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
            return HandleExceptionResult<AuthorityUser>("Failed to create user.", e);
        }
    }

    public async Task<Result<AuthorityUser>> UpdateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken: cancellationToken);
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
            return HandleExceptionResult<AuthorityUser>("Failed to update user!", e);
        }
    }

    public async Task<Result> DeleteUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken: cancellationToken);
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
            return HandleException("Failed to delete user!", e);
        }
    }

    public async Task<Result<AuthorityUser>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = await context.Users.Where(u => u.EmailAddress == email).Include(u => u.Attributes).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            return Result<AuthorityUser>.HandleResult(usr, "User not found by given email address.");
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityUser>("An error occured while getting the user.", e);
        }
    }

    public async Task<Result> SetVersionAsync(AuthorityUser user, long version, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
            if (usr == null)
            {
                return Result.Failed("Failed to find user with given id!");
            }

            if (usr.Version != user.Version)
            {
                return Result.Failed("Stored user version doesn't match current user version!");
            }
            
            usr.Version = version;
            context.Users.Update(usr);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result.Failed("Failed to update user!") : Result.Ok();
        }
        catch (Exception e)
        {
            return HandleException("An error occured while updating the version.", e);
        }
    }

    public async Task<Result<long>> GetVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usrVersion = await context.Users.Where(u => u.Id == user.Id).Select(u => u.Version).FirstOrDefaultAsync(cancellationToken);
            return Result<long>.HandleResult(usrVersion, "Failed to get user version!");
        }
        catch (Exception e)
        {
            return HandleExceptionResult<long>("An error occured while getting the user version.", e);
        }
    }

    public async Task<Result> SetSecurityVersionAsync(AuthorityUser user, long securityVersion, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
            if (usr == null)
            {
                return Result.Failed("Failed to find user with given id!");
            }

            if (usr.SecurityVersion != user.SecurityVersion)
            {
                return Result.Failed("Stored user version doesn't match current user version!");
            }
            
            usr.SecurityVersion = securityVersion;
            context.Users.Update(usr);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result.Failed("Failed to update user!") : Result.Ok();
        }
        catch (Exception e)
        {
            return HandleException("An error occured while updating the security version.", e);
        }
    }

    public async Task<Result<long>> GetSecurityVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usrVersion = await context.Users.Where(u => u.Id == user.Id).Select(u => u.SecurityVersion).FirstOrDefaultAsync(cancellationToken);
            return Result<long>.HandleResult(usrVersion, "Failed to get user security version!");
        }
        catch (Exception e)
        {
            return HandleExceptionResult<long>("An error occured while getting the user security version.", e);
        }
    }
}