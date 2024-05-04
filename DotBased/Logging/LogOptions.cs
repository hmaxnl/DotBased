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

    /// <summary>
    /// The function that will build and return the <see cref="ILogger"/> when calling <see cref="LogService.RegisterLogger"/>, so a custom logger can be used.
    /// </summary>
    public Func<string, CallingSource, Action<LogCapsule>, ILogger> LoggerBuilder { get; set; } =
        (identifier, source, sendEvent) => new Logger(identifier, source, ref sendEvent);
}