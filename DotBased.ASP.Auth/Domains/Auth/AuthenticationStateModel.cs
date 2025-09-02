using DotBased.ASP.Auth.Domains.Identity;

namespace DotBased.ASP.Auth.Domains.Auth;

public class AuthenticationStateModel
{
    public AuthenticationStateModel(UserModel user)
    {
        UserId = user.Id;
    }
    
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;

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