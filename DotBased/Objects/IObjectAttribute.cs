namespace DotBased.Objects;

public interface IObjectAttribute<TValueType>
{
    public string Key { get; set; }
    public TValueType? Value { get; set; }
}