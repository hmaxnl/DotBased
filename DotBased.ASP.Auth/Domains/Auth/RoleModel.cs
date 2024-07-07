namespace DotBased.ASP.Auth.Domains.Auth;

public class RoleModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<PermissionModel> Permissions { get; set; } = [];
}