using Microsoft.Extensions.Logging;

namespace DotBased.Logging.MEL;

public class BasedLogger : Microsoft.Extensions.Logging.ILogger
{
    private const string _messageTemplateKey = "{OriginalFormat}";
    public BasedLogger(ILogger logger)
    {
        basedLogger = logger;
    }

    private readonly ILogger basedLogger;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;
        var severity = ConvertLogLevelToSeverity(logLevel);
        var capsule = ConstructCapsule(severity, eventId, state, exception, formatter);
        basedLogger.Log(capsule);
    }

    private LogCapsule ConstructCapsule<TState>(LogSeverity severity, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var msgTemplate = string.Empty;
        List<object?> templateParams = [];
        if (state is IEnumerable<KeyValuePair<string, object>> stateEnum)
        {
            foreach (var prop in stateEnum)
            {
                if (prop is { Key: _messageTemplateKey, Value: string propValueString })
                {
                    msgTemplate = propValueString;
                    continue;
                }
                templateParams.Add(prop.Value);
            }
        }

        return new LogCapsule()
        {
            Exception = exception,
            Message = msgTemplate,
            Parameters = templateParams.ToArray(),
            Severity = severity,
            TimeStamp = DateTime.Now,
            Logger = basedLogger as LoggerBase ?? throw new NullReferenceException(nameof(basedLogger))
        };
    }

    private LogSeverity ConvertLogLevelToSeverity(LogLevel level)
    {
        return level switch
        {
            LogLevel.Trace => LogSeverity.Trace,
            LogLevel.Debug => LogSeverity.Debug,
            LogLevel.Information => LogSeverity.Info,
            LogLevel.Warning => LogSeverity.Warning,
            LogLevel.Error => LogSeverity.Error,
            LogLevel.Critical => LogSeverity.Fatal,
            LogLevel.None => LogSeverity.Ignore,
            _ => LogSeverity.Verbose
        };
    }

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;
}