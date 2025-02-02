namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityAttribute(string attributeKey, Guid bound)
{
    public AuthorityAttribute() : this(string.Empty, Guid.NewGuid())
    {
    }

    public Guid BoundId { get; set; } = bound;
    
    public string AttributeKey { get; set; } = attributeKey;

    public string AttributeValue { get; set; } = string.Empty;
    
    public string? Type { get; set; }

    public long Version { get; set; }
}