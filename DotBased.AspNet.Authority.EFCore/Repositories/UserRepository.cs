using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class UserRepository(IDbContextFactory<AuthorityContext> contextFactory, ILogger<UserRepository> logger) : RepositoryBase, IUserRepository
{
    public async Task<QueryItems<AuthorityUserItem>> GetAuthorityUsersAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default)
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
        return QueryItems<AuthorityUserItem>.Create(selected, totalCount, limit, offset);
    }

    public async Task<AuthorityUser?> GetAuthorityUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        if (id == Guid.Empty)
        {
            throw new Exception("Id is required!");
        }

        return await context.Users.Where(u => u.Id == id).Include(u => u.Attributes).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<AuthorityUser?> CreateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        if (user.Id == Guid.Empty)
        {
            throw new Exception("User id is required!");
        }
        var entity = context.Users.Add(user);
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0 ? entity.Entity : null;
    }

    public async Task<AuthorityUser?> UpdateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var usr = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken: cancellationToken);
        if (usr == null)
        {
            throw new Exception("User not found!");
        }

        if (usr.Version != user.Version || usr.SecurityVersion != user.SecurityVersion)
        {
            throw new Exception("User does not have the correct security version!");
        }
            
        var entity = context.Users.Update(user);
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0 ? entity.Entity : null;
    }

    public async Task<bool> DeleteUsersAsync(List<AuthorityUser> users, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var usrIds = users.Select(u => u.Id);
        
        context.Users.RemoveRange(users);
        context.RoleLinks.RemoveRange(context.RoleLinks.Where(rl => usrIds.Contains(rl.LinkId)));
        
        var removedResult = await context.SaveChangesAsync(cancellationToken);
        if (removedResult != 0) return true;
        logger.LogError("Failed to delete users");
        return false;
    }

    public async Task<AuthorityUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Users.Where(u => u.EmailAddress == email).Include(u => u.Attributes).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> SetVersionAsync(AuthorityUser user, long version, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var usr = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
        if (usr == null)
        {
            throw new Exception("User not found!");
        }

        if (usr.Version != user.Version)
        {
            throw new Exception("User does not have the correct security version!");
        }
            
        usr.Version = version;
        context.Users.Update(usr);
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0;
    }

    public async Task<long> GetVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var usrVersion = await context.Users.Where(u => u.Id == user.Id).Select(u => u.Version).FirstOrDefaultAsync(cancellationToken);
        return usrVersion;
    }

    public async Task<bool> SetSecurityVersionAsync(AuthorityUser user, long securityVersion, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var usr = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
        if (usr == null)
        {
            throw new Exception("User not found!");
        }

        if (usr.SecurityVersion != user.SecurityVersion)
        {
            throw new Exception("User does not have the correct security version!");
        }
            
        usr.SecurityVersion = securityVersion;
        context.Users.Update(usr);
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0;
    }

    public async Task<long> GetSecurityVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var usrVersion = await context.Users.Where(u => u.Id == user.Id).Select(u => u.SecurityVersion).FirstOrDefaultAsync(cancellationToken);
        return usrVersion;
    }
}