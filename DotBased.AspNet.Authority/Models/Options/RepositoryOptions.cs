namespace DotBased.AspNet.Authority.Models.Options;

public class RepositoryOptions
{
    /// <summary>
    /// Use data encryption when a property has the <see cref="DotBased.AspNet.Authority.Attributes.ProtectAttribute"/> defined.
    /// <value>Default: true</value>
    /// </summary>
    public bool UseDataProtection { get; set; } = true;
}