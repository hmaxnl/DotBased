using DotBased.ASP.Auth.Domains.Auth;
using DotBased.Objects;

namespace DotBased.ASP.Auth.Domains.Identity;

public class GroupModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RoleModel> Roles { get; set; } = [];
    public List<DbObjectAttribute<IConvertible>> Attributes { get; set; } = [];
}