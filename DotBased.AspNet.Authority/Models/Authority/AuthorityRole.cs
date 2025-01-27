namespace DotBased.AspNet.Authority.Models.Authority;

public abstract class AuthorityRole()
{
    public AuthorityRole(string name) : this()
    {
        Name = name;
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Name { get; set; }

    public long Version { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public override string ToString() => Name ?? string.Empty;
}