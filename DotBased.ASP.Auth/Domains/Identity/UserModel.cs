namespace DotBased.ASP.Auth.Domains.Identity;

public class UserModel
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string PasswordHash { get; set; } = string.Empty;
    public string[] GroupIds { get; set; } = Array.Empty<string>();
    public string[] Roles { get; set; } = Array.Empty<string>();
}