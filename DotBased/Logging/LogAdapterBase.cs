namespace DotBased.Logging;

/// <summary>
/// The base for creating log adpaters.
/// </summary>
public abstract class LogAdapterBase
{
    public LogAdapterBase(string adapterName)
    {
        AdapterName = adapterName;
        HandleLogEvent += HandleLog;
    }

    internal readonly EventHandler<LogCapsule> HandleLogEvent;
    public string Id { get; } = Guid.NewGuid().ToString();
    /// <summary>
    /// The name this adapter has.
    /// </summary>
    public string AdapterName { get; }

    /// <summary>
    /// Handle the incomming <see cref="LogCapsule"/> that the <see cref="LogProcessor"/> sends.
    /// </summary>
    /// <param name="processor">The log processor that has processed this log</param>
    /// <param name="capsule">The log capsule, which contains the log information</param>
    public abstract void HandleLog(object? processor, LogCapsule? capsule);

    public override int GetHashCode() => HashCode.Combine(Id, AdapterName);

    public override bool Equals(object? obj)
    {
        if (obj is LogAdapterBase adapter)
            return adapter.Id == Id;
        return false;
    }
}