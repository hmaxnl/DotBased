using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IUserRepository
{
    public Task<QueryItems<AuthorityUserItem>> GetUsersAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<AuthorityUser?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<AuthorityUser?> CreateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<AuthorityUser?> UpdateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<bool> DeleteUsersAsync(List<AuthorityUser> users, CancellationToken cancellationToken = default);
    public Task<AuthorityUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<bool> SetVersionAsync(AuthorityUser user, long version, CancellationToken cancellationToken = default);
    public Task<long> GetVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<bool> SetSecurityVersionAsync(AuthorityUser user, long securityVersion, CancellationToken cancellationToken = default);
    public Task<long> GetSecurityVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default);
}