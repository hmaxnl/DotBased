using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class UserRepository : IUserRepository
{
    public Task<ListResult<AuthorityUserItem>> GetAuthorityUsersAsync(int limit = 20, int offset = 0, string search = "",
        CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
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