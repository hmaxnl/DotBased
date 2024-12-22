namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityAttribute
{
    public AuthorityAttribute(string attributeKey, string bound) : this()
    {
        AttributeKey = attributeKey;
        BoundId = bound;
    }

    public AuthorityAttribute()
    {
        
    }

    public string AttributeKey { get; set; } // ClaimType/Authority.attribute.enabled
    
    public string BoundId { get; set; } // Bound to User, Group, Role id

    public string? AttributeValue { get; set; }
    
    public string? Type { get; set; } // AspNet.Claim.Role/Property/Data.JSON, Data.Raw, Data.Base64 etc.

    public long Version { get; set; }
}