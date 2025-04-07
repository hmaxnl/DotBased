using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class UserRepository(IDbContextFactory<AuthorityContext> contextFactory) : RepositoryBase, IUserRepository
{
    public async Task<ListResultOld<AuthorityUserItem>> GetAuthorityUsersAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
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
            return ListResultOld<AuthorityUserItem>.Ok(selected, totalCount, limit, offset);
        }
        catch (Exception e)
        {
            return HandleExceptionListResult<AuthorityUserItem>("Failed to get users.", e);
        }
    }

    public async Task<ResultOld<AuthorityUser>> GetAuthorityUserByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            if (!Guid.TryParse(id, out var guid))
            {
                return ResultOld<AuthorityUser>.Failed("Invalid id!");
            }

            var user = await context.Users.Where(u => u.Id == guid).Include(u => u.Attributes).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            return ResultOld<AuthorityUser>.HandleResult(user, "User not found.");
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityUser>("Failed to get user.", e);
        }
    }

    public async Task<ResultOld<AuthorityUser>> CreateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            if (user.Id == Guid.Empty)
            {
                return ResultOld<AuthorityUser>.Failed("Id cannot be empty!");
            }
            var entity = context.Users.Add(user);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? ResultOld<AuthorityUser>.Failed("Failed to create user!") : ResultOld<AuthorityUser>.Ok(entity.Entity);
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityUser>("Failed to create user.", e);
        }
    }

    public async Task<ResultOld<AuthorityUser>> UpdateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken: cancellationToken);
            if (usr == null)
            {
                return ResultOld<AuthorityUser>.Failed("User not found!");
            }

            if (usr.Version != user.Version || usr.SecurityVersion != user.SecurityVersion)
            {
                return ResultOld<AuthorityUser>.Failed("Version validation failed!");
            }
            
            var entity = context.Users.Update(user);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? ResultOld<AuthorityUser>.Failed("Failed to save updated user!") : ResultOld<AuthorityUser>.Ok(entity.Entity);
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityUser>("Failed to update user!", e);
        }
    }

    public async Task<ResultOld> DeleteUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken: cancellationToken);
            if (usr == null)
            {
                return ResultOld.Failed("User not found!");
            }
            context.Users.Remove(usr);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? ResultOld.Failed("Failed to delete user!") : ResultOld.Ok();
        }
        catch (Exception e)
        {
            return HandleException("Failed to delete user!", e);
        }
    }

    public async Task<ResultOld<AuthorityUser>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = await context.Users.Where(u => u.EmailAddress == email).Include(u => u.Attributes).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            return ResultOld<AuthorityUser>.HandleResult(usr, "User not found by given email address.");
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityUser>("An error occured while getting the user.", e);
        }
    }

    public async Task<ResultOld> SetVersionAsync(AuthorityUser user, long version, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
            if (usr == null)
            {
                return ResultOld.Failed("Failed to find user with given id!");
            }

            if (usr.Version != user.Version)
            {
                return ResultOld.Failed("Stored user version doesn't match current user version!");
            }
            
            usr.Version = version;
            context.Users.Update(usr);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? ResultOld.Failed("Failed to update user!") : ResultOld.Ok();
        }
        catch (Exception e)
        {
            return HandleException("An error occured while updating the version.", e);
        }
    }

    public async Task<ResultOld<long>> GetVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usrVersion = await context.Users.Where(u => u.Id == user.Id).Select(u => u.Version).FirstOrDefaultAsync(cancellationToken);
            return ResultOld<long>.HandleResult(usrVersion, "Failed to get user version!");
        }
        catch (Exception e)
        {
            return HandleExceptionResult<long>("An error occured while getting the user version.", e);
        }
    }

    public async Task<ResultOld> SetSecurityVersionAsync(AuthorityUser user, long securityVersion, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usr = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
            if (usr == null)
            {
                return ResultOld.Failed("Failed to find user with given id!");
            }

            if (usr.SecurityVersion != user.SecurityVersion)
            {
                return ResultOld.Failed("Stored user version doesn't match current user version!");
            }
            
            usr.SecurityVersion = securityVersion;
            context.Users.Update(usr);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? ResultOld.Failed("Failed to update user!") : ResultOld.Ok();
        }
        catch (Exception e)
        {
            return HandleException("An error occured while updating the security version.", e);
        }
    }

    public async Task<ResultOld<long>> GetSecurityVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var usrVersion = await context.Users.Where(u => u.Id == user.Id).Select(u => u.SecurityVersion).FirstOrDefaultAsync(cancellationToken);
            return ResultOld<long>.HandleResult(usrVersion, "Failed to get user security version!");
        }
        catch (Exception e)
        {
            return HandleExceptionResult<long>("An error occured while getting the user security version.", e);
        }
    }
}