namespace DotBased.Logging;

/// <summary>
/// Options for loggers, processor and <see cref="LogService"/>.
/// </summary>
public class LogOptions
{
    /// <summary>
    /// The severty the logger will log
    /// </summary>
    public LogSeverity Severity { get; set; } = LogSeverity.Trace;
}