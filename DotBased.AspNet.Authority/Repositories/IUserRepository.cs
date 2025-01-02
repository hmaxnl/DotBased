namespace DotBased.AspNet.Authority.Repositories;

public interface IUserRepository<TUser> where TUser : class
{
    public Task<TUser?> GetUserByIdAsync(string id);
    public Task<string> GetUserIdAsync(TUser user);
    public Task<TUser?> GetUserByEmailAsync(string email);
    public Task SetVersionAsync(TUser user, long version);
    public Task<long> GetVersionAsync(TUser user);
    public Task SetSecurityVersionAsync(TUser user, long version);
    public Task<long> GetSecurityVersionAsync(TUser user);
    public Task<TUser?> CreateUserAsync(TUser user);
    public Task<TUser?> UpdateUserAsync(TUser user);
    public Task<bool> DeleteUserAsync(TUser user);
}