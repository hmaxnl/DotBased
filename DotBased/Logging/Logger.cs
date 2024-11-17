namespace DotBased.Logging;

/// <summary>
/// Main logger, this class is the default logger that the <see cref="LogService.RegisterLogger"/> function will return.
/// </summary>
public class Logger(LoggerInformation loggerInformation, string name) : LoggerBase(loggerInformation, name)
{
    public override void Verbose(string message, params object?[]? parameters)
    {
        Log(new LogCapsule()
        {
            Logger = this,
            Message = message,
            Parameters = parameters,
            Severity = LogSeverity.Verbose,
            TimeStamp = DateTime.Now
        });
    }
    
    public override void Trace(string message, params object?[]? parameters)
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

    public override void Debug(string message, params object?[]? parameters)
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

    public override void Information(string message, params object?[]? parameters)
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

    public override void Warning(string message, params object?[]? parameters)
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

    public override void Error(Exception exception, string message, params object?[]? parameters)
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

    public override void Fatal(Exception exception, string message, params object?[]? parameters)
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

    public override int GetHashCode() => HashCode.Combine(LoggerInformation.TypeFullName, LoggerInformation.AssemblyFullname);
}