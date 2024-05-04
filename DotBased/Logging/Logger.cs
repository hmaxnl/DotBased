namespace DotBased.Logging;

/// <summary>
/// Main base logger, this class is the default logger that the <see cref="LogService.RegisterLogger"/> function will return.
/// </summary>
public class Logger(string identifier, CallingSource source, ref Action<LogCapsule> logProcessorHandler) : ILogger
{
    public string Identifier { get; } = identifier;
    public CallingSource Source { get; } = source;

    private readonly Action<LogCapsule> _processLog = logProcessorHandler;
    
    public void Log(LogCapsule capsule)
    {
        _processLog(capsule);
    }

    public void Trace(string message, params object?[]? parameters)
    {
        Log(new LogCapsule()
        {
            Logger = this,
            Message = message,
            Parameters = parameters,
            Severity = LogSeverity.Trace,
            TimeStamp = DateTime.Now
        });
    }

    public void Debug(string message, params object?[]? parameters)
    {
        Log(new LogCapsule()
        {
            Logger = this,
            Message = message,
            Parameters = parameters,
            Severity = LogSeverity.Debug,
            TimeStamp = DateTime.Now
        });
    }

    public void Information(string message, params object?[]? parameters)
    {
        Log(new LogCapsule()
        {
            Logger = this,
            Message = message,
            Parameters = parameters,
            Severity = LogSeverity.Info,
            TimeStamp = DateTime.Now
        });
    }

    public void Warning(string message, params object?[]? parameters)
    {
        Log(new LogCapsule()
        {
            Logger = this,
            Message = message,
            Parameters = parameters,
            Severity = LogSeverity.Warning,
            TimeStamp = DateTime.Now
        });
    }

    public void Error(Exception exception, string message, params object?[]? parameters)
    {
        Log(new LogCapsule()
        {
            Logger = this,
            Message = message,
            Parameters = parameters,
            Severity = LogSeverity.Error,
            TimeStamp = DateTime.Now,
            Exception = exception
        });
    }

    public void Fatal(Exception exception, string message, params object?[]? parameters)
    {
        Log(new LogCapsule()
        {
            Logger = this,
            Message = message,
            Parameters = parameters,
            Severity = LogSeverity.Fatal,
            TimeStamp = DateTime.Now,
            Exception = exception
        });
    }

    public override int GetHashCode() => HashCode.Combine(Identifier, Source.AssemblyFullName);
}