using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class UserRepository(IDbContextFactory<AuthorityContext> contextFactory) : IUserRepository
{
    public async Task<ListResult<AuthorityUserItem>> GetAuthorityUsersAsync(int limit = 20, int offset = 0, string search = "", CancellationToken? cancellationToken = null)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var query = context.Users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u => $"{u.Id} {u.Name} {u.UserName} {u.EmailAddress} {u.PhoneNumber}".Contains(search, StringComparison.CurrentCultureIgnoreCase));
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

    public Task<Result<AuthorityUser>> GetAuthorityUserByIdAsync(string id, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityUser>> CreateUserAsync(AuthorityUser user, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityUser>> UpdateUserAsync(AuthorityUser user, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteUserAsync(AuthorityUser user, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthorityUser>> GetUserByEmailAsync(string email, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SetVersionAsync(AuthorityUser user, long version, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result<long>> GetVersionAsync(AuthorityUser user, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SetSecurityVersionAsync(AuthorityUser user, long version, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result<long>> GetSecurityVersionAsync(AuthorityUser user, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }
}