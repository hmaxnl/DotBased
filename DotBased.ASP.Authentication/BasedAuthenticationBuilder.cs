namespace DotBased.ASP.Authentication;

public class BasedAuthenticationBuilder
{
    public BasedAuthenticationBuilder(Type authRepository)
    {
        if (authRepository.IsValueType)
        {
            throw new ArgumentException("Type cannot be a value type!", nameof(authRepository));
        }

        AuthenticationRepositoryType = authRepository;
    }

    public Type AuthenticationRepositoryType { get; }
}