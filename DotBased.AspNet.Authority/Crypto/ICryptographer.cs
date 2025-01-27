namespace DotBased.AspNet.Authority.Crypto;

public interface ICryptographer
{
    public Task<string?> EncryptAsync(string data);
    public Task<string?> DecryptAsync(string data);
}