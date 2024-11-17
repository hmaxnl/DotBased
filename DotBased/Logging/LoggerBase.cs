using DotBased.Extensions;

namespace DotBased.Logging;

/// <summary>
/// Base for creating loggers
/// </summary>
/// <param name="loggerInformation">The caller information</param>
public abstract class LoggerBase(LoggerInformation loggerInformation, string name) : ILogger
{
    public LoggerInformation LoggerInformation { get; } = loggerInformation;
    public string Name { get; } = name.IsNullOrEmpty() ? loggerInformation.TypeNamespace : name;

    private readonly Action<LogCapsule> ProcessLog = LogService.LoggerSendEvent;
    
    public void Log(LogCapsule capsule)
    {
        ProcessLog(capsule);
    }
    

    public abstract void Verbose(string message, params object?[]? parameters);
    public abstract void Trace(string message, params object?[]? parameters);
    public abstract void Debug(string message, params object?[]? parameters);
    public abstract void Information(string message, params object?[]? parameters);
    public abstract void Warning(string message, params object?[]? parameters);
    public abstract void Error(Exception exception, string message, params object?[]? parameters);
    public abstract void Fatal(Exception exception, string message, params object?[]? parameters);
}