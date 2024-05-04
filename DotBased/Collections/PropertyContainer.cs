using System.Collections.ObjectModel;

namespace DotBased.Collections;

/// <summary>
/// Property container to store string, long, int, double and bool properties
/// </summary>
public class PropertyContainer : KeyContainerBase<PropertyContainer>
{
    public PropertyContainer()
    { }
    public PropertyContainer(char separator) : base(separator)
    { }

    private readonly Dictionary<string, object> _data = new Dictionary<string, object>();
    public ReadOnlyDictionary<string, object> Data => new ReadOnlyDictionary<string, object>(_data);

    /// <summary>
    /// Set a property with the corresponding key.
    /// </summary>
    /// <param name="key">The key that will correspond to the property</param>
    /// <param name="sValue">The string property value</param>
    /// <param name="overridable">If the key already exists overwrite the property</param>
    public void Set(string key, string sValue, bool overridable = true)
    {
        if (ContainsKey(key) && !overridable)
            return;
        _data[key] = sValue;
    }

    public void Set(string key, long lValue, bool overridable = true)
    {
        if (ContainsKey(key) && !overridable)
            return;
        _data[key] = lValue;
    }

    public void Set(string key, double dValue, bool overridable = true)
    {
        if (ContainsKey(key) && !overridable)
            return;
        _data[key] = dValue;
    }

    public void Set(string key, bool bValue, bool overridable = true)
    {
        if (ContainsKey(key) && !overridable)
            return;
        _data[key] = bValue;
    }

    /// <summary>
    /// Check if the key exists in this container.
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key exists otherwise false</returns>
    public bool ContainsKey(string key) => _data.ContainsKey(key);

    public string GetString(string key) => Convert.ToString(_data[key]) ?? string.Empty;
    public long GetLong(string key) => Convert.ToInt64(_data[key]);
    public double GetDouble(string key) => Convert.ToDouble(_data[key]);
    public bool GetBool(string key) => Convert.ToBoolean(_data[key]);

    /// <summary>
    /// Removes the property at the passed key.
    /// </summary>
    /// <param name="key">The key where to remove the property</param>
    /// <returns>True is the property is removed otherwise false</returns>
    public bool Remove(string key) => _data.Remove(key);
    /// <summary>
    /// Clears all the properties in this container.
    /// </summary>
    public void ClearData() => _data.Clear();
}