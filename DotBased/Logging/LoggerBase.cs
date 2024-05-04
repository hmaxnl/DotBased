namespace DotBased.Logging;

/// <summary>
/// Base for creating loggers
/// </summary>
/// <param name="caller">The caller information</param>
/// <param name="logProcessorHandler">The handler where the logs can be send to</param>
public abstract class LoggerBase(CallerInformation caller, ref Action<LogCapsule> logProcessorHandler) : ILogger
{
    public CallerInformation Caller { get; } = caller;

    internal readonly Action<LogCapsule> ProcessLog = logProcessorHandler;

    public abstract void Trace(string message, params object?[]? parameters);
    public abstract void Debug(string message, params object?[]? parameters);
    public abstract void Information(string message, params object?[]? parameters);
    public abstract void Warning(string message, params object?[]? parameters);
    public abstract void Error(Exception exception, string message, params object?[]? parameters);
    public abstract void Fatal(Exception exception, string message, params object?[]? parameters);
}