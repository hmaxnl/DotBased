using DotBased.Extensions;

namespace DotBased.Objects;

public class DbObjectAttribute<TValueType> : ObjectAttribute<TValueType> where TValueType : IConvertible
{
    public DbObjectAttribute(string key, TValueType value, string owner) : base(key, value)
    {
        if (owner.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(owner), $"The parameter {nameof(owner)} is null or empty! This parameter is required!");
        Owner = owner;
    }
    public string Owner { get; set; }
}