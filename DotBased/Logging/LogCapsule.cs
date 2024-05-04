namespace DotBased.Logging;

/// <summary>
/// This will contain all the log event information that the log adapter will receive.
/// </summary>
public class LogCapsule
{
    /// <summary>
    /// The log serverty this log event is being logged.
    /// </summary>
    public LogSeverity Severity { get; set; }
    public string Message { get; set; } = string.Empty;
    public Exception? Exception { get; set; }
    public object?[]? Parameters { get; set; }
    /// <summary>
    /// Time stamp on when this event happend
    /// </summary>
    public DateTime TimeStamp { get; set; }
    /// <summary>
    /// The logger that generated this capsule
    /// </summary>
    public ILogger Logger { get; set; }
}