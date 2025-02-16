namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityRoleItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Name { get; set; }
}