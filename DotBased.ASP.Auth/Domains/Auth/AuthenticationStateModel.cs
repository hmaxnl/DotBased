namespace DotBased.ASP.Auth.Domains.Auth;

public class AuthenticationStateModel
{
    public string Id { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is AuthenticationStateModel authStateModel)
            return authStateModel.Id == Id;
        return false;
    }
    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => Id;
}