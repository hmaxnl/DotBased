namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityRole()
{
    public AuthorityRole(string name) : this()
    {
        Name = name;
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public long Version { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public IEnumerable<AuthorityAttribute> Attributes { get; set; } = [];

    public override string ToString() => Name;
}