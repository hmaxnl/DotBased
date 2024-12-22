namespace DotBased.AspNet.Authority.Interfaces;

public interface IUserRepository<TUser, TId> : IVersionRepository<TUser>, ISecurityVersionRepository<TUser> where TUser : class where TId : IEquatable<TId>
{
    public Task<TUser?> GetUserByIdAsync(TId id);

    public Task<TId> GetUserIdAsync(TUser user);
}