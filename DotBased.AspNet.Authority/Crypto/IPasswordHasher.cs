namespace DotBased.AspNet.Authority.Crypto;

public interface IPasswordHasher
{
    public Task<string> HashPasswordAsync(string password);
}