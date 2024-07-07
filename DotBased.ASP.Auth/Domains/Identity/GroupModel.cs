using DotBased.ASP.Auth.Domains.Auth;

namespace DotBased.ASP.Auth.Domains.Identity;

public class GroupModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RoleModel> Roles { get; set; } = [];
}