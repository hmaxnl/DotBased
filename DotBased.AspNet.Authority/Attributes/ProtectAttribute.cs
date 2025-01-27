namespace DotBased.AspNet.Authority.Attributes;

/// <summary>
/// Indicates to protect the property before saving/loading to the repository.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ProtectAttribute : Attribute
{
    
}