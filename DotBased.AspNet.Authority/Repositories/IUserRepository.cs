namespace DotBased.AspNet.Authority.Repositories;

public interface IUserRepository<TUser> where TUser : class
{
    public Task<TUser?> GetUserByIdAsync(string id);
    public Task<string> GetUserIdAsync(TUser user);
    public Task SetVersion(TUser user, long version);
    public Task SetSecurityVersion(TUser user, long version);
}