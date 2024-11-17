using System.Collections.ObjectModel;
using DotBased.Extensions;

namespace DotBased.Logging;

/// <summary>
/// Options for loggers, processor and <see cref="LogService"/>.
/// </summary>
public class LogOptions
{
    public readonly SeverityFilterCollection SeverityFilters = [];
    
    /// <summary>
    /// The severity the logger will log
    /// </summary>
    public LogSeverity Severity { get; set; } = LogSeverity.Trace;

    /// <summary>
    /// The function that will build and return the <see cref="ILogger"/>.
    /// </summary>
    public Func<LoggerInformation, string, ILogger> LoggerBuilder { get; set; } =
        (identifier, loggerName) => new Logger(identifier, loggerName);

    public LogOptions AddSeverityFilter(string filter, LogSeverity logSeverity)
    {
        if (filter.IsNullOrEmpty())
            return this;
        SeverityFilters.Add(new SeverityFilter() { Filter = filter, Severity = logSeverity });
        return this;
    }
}

public struct SeverityFilter
{
    public string Filter { get; set; }
    public LogSeverity Severity { get; set; }
}

public class SeverityFilterCollection : KeyedCollection<string, SeverityFilter>
{
    protected override string GetKeyForItem(SeverityFilter item) => item.Filter;
}