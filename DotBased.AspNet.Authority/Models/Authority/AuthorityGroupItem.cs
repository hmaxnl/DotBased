namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityGroupItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Name { get; set; }
}