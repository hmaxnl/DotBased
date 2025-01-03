namespace DotBased.AspNet.Authority.Repositories;

public interface IUserRepository<TUser> where TUser : class
{
    public Task<TUser?> GetUserByIdAsync(string id, CancellationToken? cancellationToken = null);
    public Task<string> GetUserIdAsync(TUser user, CancellationToken? cancellationToken = null);
    public Task<Tuple<List<TUser>?, int>> GetUsersAsync(string query, int maxResults = 20, int offset = 0, CancellationToken? cancellationToken = null);
    public Task<TUser?> GetUserByEmailAsync(string email, CancellationToken? cancellationToken = null);
    public Task SetVersionAsync(TUser user, long version, CancellationToken? cancellationToken = null);
    public Task<long> GetVersionAsync(TUser user, CancellationToken? cancellationToken = null);
    public Task SetSecurityVersionAsync(TUser user, long version, CancellationToken? cancellationToken = null);
    public Task<long> GetSecurityVersionAsync(TUser user, CancellationToken? cancellationToken = null);
    public Task<TUser?> CreateUserAsync(TUser user, CancellationToken? cancellationToken = null);
    public Task<TUser?> UpdateUserAsync(TUser user, CancellationToken? cancellationToken = null);
    public Task<bool> DeleteUserAsync(TUser user, CancellationToken? cancellationToken = null);
}