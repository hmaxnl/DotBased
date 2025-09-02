namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityAttribute(string attributeKey, Guid foreignKey)
{
    public Guid ForeignKey { get; set; } = foreignKey;
    
    public string AttributeKey { get; set; } = attributeKey;

    public string AttributeValue { get; set; } = string.Empty;
    
    public string? Type { get; set; }

    public long Version { get; set; }
}