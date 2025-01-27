using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IUserRepository
{
    public Task<AuthorityUser?> GetAuthorityUserByIdAsync(string id, CancellationToken? cancellationToken = null);
    public Task<string> GetAuthorityUserIdAsync(AuthorityUser user, CancellationToken? cancellationToken = null);
    public Task<Tuple<List<AuthorityUser>?, int>> GetAuthorityUsersAsync(string query, int maxResults = 20, int offset = 0, CancellationToken? cancellationToken = null);
    public Task<AuthorityUser?> GetAuthorityUserByEmailAsync(string email, CancellationToken? cancellationToken = null);
    public Task SetVersionAsync(AuthorityUser user, long version, CancellationToken? cancellationToken = null);
    public Task<long> GetVersionAsync(AuthorityUser user, CancellationToken? cancellationToken = null);
    public Task SetSecurityVersionAsync(AuthorityUser user, long version, CancellationToken? cancellationToken = null);
    public Task<long> GetSecurityVersionAsync(AuthorityUser user, CancellationToken? cancellationToken = null);
    public Task<AuthorityUser?> CreateUserAsync(AuthorityUser user, CancellationToken? cancellationToken = null);
    public Task<AuthorityUser?> UpdateUserAsync(AuthorityUser user, CancellationToken? cancellationToken = null);
    public Task<bool> DeleteUserAsync(AuthorityUser user, CancellationToken? cancellationToken = null);
}