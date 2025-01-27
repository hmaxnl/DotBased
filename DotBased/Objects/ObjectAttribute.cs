using DotBased.Extensions;

namespace DotBased.Objects;

public class ObjectAttribute<TValueType> : IObjectAttribute<TValueType>
{
    protected ObjectAttribute(string key, TValueType value)
    {
        if (key.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(key), $"The parameter {nameof(key)} is null or empty!");
        Key = key;
        Value = value;
    }
    public string Key { get; set; }
    public TValueType? Value { get; set; }
}