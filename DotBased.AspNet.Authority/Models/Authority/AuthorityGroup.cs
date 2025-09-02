namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityGroup()
{
    public AuthorityGroup(string name) : this()
    {
        Name = name;
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Name { get; set; }

    public long Version { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public ICollection<AuthorityAttribute> Attributes { get; set; } = [];
}