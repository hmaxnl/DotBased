using DotBased.AspNet.Authority.Models.Authority;

namespace DotBased.AspNet.Authority.Repositories;

public interface IUserRepository
{
    public Task<ListResult<AuthorityUserItem>> GetAuthorityUsersAsync(int limit = 20, int offset = 0, string search = "", CancellationToken cancellationToken = default);
    public Task<Result<AuthorityUser>> GetAuthorityUserByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityUser>> CreateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityUser>> UpdateUserAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<Result> DeleteUserAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<Result<AuthorityUser>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<Result> SetVersionAsync(AuthorityUser user, long version, CancellationToken cancellationToken = default);
    public Task<Result<long>> GetVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default);
    public Task<Result> SetSecurityVersionAsync(AuthorityUser user, long version, CancellationToken cancellationToken = default);
    public Task<Result<long>> GetSecurityVersionAsync(AuthorityUser user, CancellationToken cancellationToken = default);
}