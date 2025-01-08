namespace DotBased.AspNet.Authority.Models.Authority;

public abstract class AuthorityRole
{
    public AuthorityRole(string name) : this()
    {
        Name = name;
    }
    
    public AuthorityRole()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.Now;
    }
    
    public Guid Id { get; set; }
    
    public string? Name { get; set; }

    public long Version { get; set; }

    public DateTime CreatedDate { get; set; }

    public override string ToString() => Name ?? string.Empty;
}