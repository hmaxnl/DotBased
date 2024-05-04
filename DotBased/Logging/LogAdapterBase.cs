namespace DotBased.Logging;

public abstract class LogAdapterBase
{
    public LogAdapterBase(string adapterName)
    {
        AdapterName = adapterName;
        HandleLogEvent += HandleLog;
    }

    internal readonly EventHandler<LogCapsule> HandleLogEvent;
    public string Id { get; } = Guid.NewGuid().ToString();
    public string AdapterName { get; }

    public abstract void HandleLog(object? sender, LogCapsule? capsule);

    public override int GetHashCode() => HashCode.Combine(Id, AdapterName);

    public override bool Equals(object? obj)
    {
        if (obj is LogAdapterBase adapter)
            return adapter.Id == Id;
        return false;
    }
}