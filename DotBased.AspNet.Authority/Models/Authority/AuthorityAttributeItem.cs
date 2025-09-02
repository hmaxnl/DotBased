namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityAttributeItem
{
    public Guid BoundId { get; set; }
    
    public string AttributeKey { get; set; } = string.Empty;

    public string AttributeValue { get; set; } = string.Empty;
}