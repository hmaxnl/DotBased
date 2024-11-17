using DotBased.Logging;

namespace DotBased.Collections;

/// <summary>
/// Container to store instances
/// <remarks>WIP!</remarks>
/// </summary>
public class InstanceContainer : IDisposable
{
    private readonly ILogger _log = LogService.RegisterLogger<InstanceContainer>();
    private readonly Dictionary<string, InstanceNode> _tCollection = new Dictionary<string, InstanceNode>();

    /// <summary>
    /// Register a instance.
    /// </summary>
    /// <remarks>The instace will be created by the <see cref="Get{TInstance}"/> function</remarks>
    /// <param name="key">Key to indentify the instance</param>
    /// <typeparam name="TInstance">The instance type</typeparam>
    public void Register<TInstance>(string key) => _tCollection.Add(key, new InstanceNode(null, typeof(TInstance)));

    /// <summary>
    /// Add an already constructed instance to the container.
    /// </summary>
    /// <param name="key">Key to identify instance</param>
    /// <param name="instance">Constructed instance</param>
    public void Add(string key, object instance) => _tCollection.Add(key, new InstanceNode(instance, instance.GetType()));

    /// <summary>
    /// Remove a instance from the container.
    /// </summary>
    /// <param name="key">Key to get the instance</param>
    /// <param name="dispose">Dispose the instance if it inherits the 'IDisposable' interface</param>
    public void Remove(string key, bool dispose = true)
    {
        if (!_tCollection.TryGetValue(key, out var iNode))
            return;
        switch (iNode.Instance)
        {
            case null:
                break;
            case IDisposable instance when dispose:
                _log.Debug("Disposing disposable object...");
                instance.Dispose();
                break;
        }
        _tCollection.Remove(key);
    }

    /// <summary>
    /// Get the instance that is stored in the container.
    /// </summary>
    /// <remarks>If the instance is not yet constructed this wil activate the instance</remarks>
    /// <param name="key">Key to identify the instance</param>
    /// <typeparam name="TInstance">Instasnce type</typeparam>
    /// <returns>The instance that is at the given key</returns>
    public TInstance? Get<TInstance>(string key)
    {
        if (!_tCollection.TryGetValue(key, out InstanceNode node) || node.InstanceType != typeof(TInstance))
            return default;
        if (node.Instance != null)
            return (TInstance)node.Instance;
        node.Instance = Activator.CreateInstance(node.InstanceType);
        // Override the old node with the new data, else the next 'Get' will reactivate a another instance.
        _tCollection[key] = node;
        if (node.Instance != null) return (TInstance)node.Instance;
        _log.Warning("Instance is null!");
        return default;
    }


    public void Dispose()
    {
        foreach (var kvp in _tCollection)
        {
            switch (kvp.Value.Instance)
            {
                case null:
                    continue;
                case IDisposable disposable:
                    _log.Verbose("Disposing: {Key}", kvp.Key);
                    disposable.Dispose();
                    break;
            }
        }
    }
}
internal struct InstanceNode
{
    public InstanceNode(object? instance, Type instanceType)
    {
        Instance = instance;
        InstanceType = instanceType;
    }
    public object? Instance;
    public readonly Type InstanceType;
}