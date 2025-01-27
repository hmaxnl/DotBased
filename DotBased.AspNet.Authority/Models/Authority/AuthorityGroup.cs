namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityGroup
{
    public AuthorityGroup(string name) : this()
    {
        Name = name;
    }
    
    public AuthorityGroup()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.Now;
    }
    
    public Guid Id { get; set; }
    
    public string? Name { get; set; }

    public long Version { get; set; }
    
    public DateTime CreatedDate { get; set; }
}