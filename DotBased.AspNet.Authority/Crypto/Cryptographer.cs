namespace DotBased.AspNet.Authority.Crypto;

public class Cryptographer : ICryptographer
{
    public Task<string?> EncryptAsync(string data)
    {
        throw new NotImplementedException();
    }

    public Task<string?> DecryptAsync(string data)
    {
        throw new NotImplementedException();
    }
}