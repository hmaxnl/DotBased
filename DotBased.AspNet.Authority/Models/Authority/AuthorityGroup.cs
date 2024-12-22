namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityGroup : AuthorityGroup<Guid>
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
}

public abstract class AuthorityGroup<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
    
    public string? Name { get; set; }

    public long Version { get; set; }
    
    public DateTime CreatedDate { get; set; }
}