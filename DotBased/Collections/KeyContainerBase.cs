using System.Collections.ObjectModel;

namespace DotBased.Collections;

/// <summary>
/// Base class for creating containers that has tree like keys
/// </summary>
/// <param name="separator">The separator used for the keys</param>
/// <typeparam name="TContainer">Container data value</typeparam>
public abstract class KeyContainerBase<TContainer>(char separator = '.')
    where TContainer : KeyContainerBase<TContainer>, new()
{
    private readonly Dictionary<string, TContainer> _containers = new Dictionary<string, TContainer>();
    public ReadOnlyDictionary<string, TContainer> Containers => new ReadOnlyDictionary<string, TContainer>(_containers);
        
    public TContainer this[string key]
    {
        get
        {
            if (key.Contains(separator))
                return AddFromQueue(new KeyQueue(key, separator));
            if (!_containers.ContainsKey(key))
                AddContainer(key, new TContainer());
            return _containers[key];
        }
    }
    public void AddContainer(string key, TContainer container) => _containers[key] = container;
    public bool RemoveContainer(string key) => _containers.Remove(key);
    public TContainer GetContainer(string key) => _containers[key];

    TContainer AddFromQueue(KeyQueue queue)
    {
        if (queue.IsEmpty) return (TContainer)this;
        string queueKey = queue.Next();
        AddContainer(queueKey, new TContainer());
        return _containers[queueKey].AddFromQueue(queue);
    }
}
internal class KeyQueue(string key, char divider)
{
    public string Next() => _key.Dequeue();
    public bool IsEmpty => _key.Count <= 0;

    private readonly Queue<string> _key = new Queue<string>(key.Split(divider, StringSplitOptions.RemoveEmptyEntries));
}