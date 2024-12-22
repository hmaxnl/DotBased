namespace DotBased.AspNet.Authority.Interfaces;

public interface IUserRepository<TUser, TId> where TUser : class where TId : IEquatable<TId>
{
    public Task<TUser?> GetUserByIdAsync(TId id);
    public Task<TId> GetUserIdAsync(TUser user);
    public Task SetVersion(TUser user, long version);
    public Task SetSecurityVersion(TUser user, long version);
}