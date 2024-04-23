namespace DotBased.Logging;

public class LogCapsule
{
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