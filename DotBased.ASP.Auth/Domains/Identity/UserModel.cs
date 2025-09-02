using DotBased.ASP.Auth.Domains.Auth;
using DotBased.Objects;

namespace DotBased.ASP.Auth.Domains.Identity;

public class UserModel
{
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public DateTime Dob { get; set; }

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public bool Enabled { get; set; }
    public bool EmailValidated { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool Lockout { get; set; }
    public DateTime LockoutEnd { get; set; }
    public DateTime CreationStamp { get; set; }
    public DateTime SecurityStamp { get; set; }
    public DateTime ConcurrencyStamp { get; set; }
    public int AccessFailedCount { get; set; }
    public bool ExternalAuthentication { get; set; }

    public List<GroupModel> Groups { get; set; } = [];
    public List<RoleModel> Roles { get; set; } = [];
    public List<DbObjectAttribute<IConvertible>> Attributes { get; set; } = [];
}