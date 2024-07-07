namespace DotBased.ASP.Auth.Domains;

public class RegisterModel
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
}