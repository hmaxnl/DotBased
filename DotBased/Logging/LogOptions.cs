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
    /// The function that will build and return the <see cref="LoggerBase"/> when calling <see cref="LogService.RegisterLogger"/>, so a custom logger based on the <see cref="LoggerBase"/> can be used.
    /// </summary>
    public Func<CallerInformation, Action<LogCapsule>, LoggerBase> LoggerBuilder { get; set; } =
        (identifier, sendEvent) => new Logger(identifier, ref sendEvent);
}