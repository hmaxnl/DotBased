using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IUserRepository
{
    public Task<ListResultOld<AuthorityUserItem>> GetAuthorityUsersAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<ResultOld<AuthorityUser>> GetAuthorityUserByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<ResultOld<AuthorityUser>> CreateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<ResultOld<AuthorityUser>> UpdateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<ResultOld> DeleteUserAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<ResultOld<AuthorityUser>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<ResultOld> SetVersionAsync(AuthorityUser user, long version, CancellationToken cancellationToken = default);
    public Task<ResultOld<long>> GetVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<ResultOld> SetSecurityVersionAsync(AuthorityUser user, long securityVersion, CancellationToken cancellationToken = default);
    public Task<ResultOld<long>> GetSecurityVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default);
}