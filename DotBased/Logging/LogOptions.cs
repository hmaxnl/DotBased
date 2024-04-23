namespace DotBased.Logging;

public class LogOptions
{
    /// <summary>
    /// The severty the logger will log
    /// </summary>
    public LogSeverity Severity { get; set; } = LogSeverity.Trace;
}