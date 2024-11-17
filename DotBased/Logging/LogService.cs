using System.Collections.ObjectModel;
using System.Reflection;
using DotBased.Extensions;

namespace DotBased.Logging;

/// <summary>
/// Main log service class, handles the loggers, log processor and adapters.
/// </summary>
public static class LogService
{
    static LogService()
    {
        Options = new LogOptions();
        LoggerSendEvent = LogProcessor.IncomingLogHandlerEvent;
    }
    public static bool CanLog(LogSeverity maxSeverity, LogSeverity severity) => maxSeverity <= severity;
    public static LogOptions Options { get; private set; }
    public static LogProcessor LogProcessor { get; private set; } = new LogProcessor();
    
    private static HashSet<LogAdapterBase> Adapters { get; } = [];
    private static HashSet<ILogger> Loggers { get; } = [];

    /// <summary>
    /// Action for internal communication between loggers and processor
    /// </summary>
    internal static readonly Action<LogCapsule> LoggerSendEvent;

    public static void Initialize(Action<LogOptions>? options = null)
    {
        Options = new LogOptions();
        options?.Invoke(Options);
    }

    public static ILogger RegisterLogger(Type? callerType, string name = "")
    {
        var logger = Options.LoggerBuilder.Invoke(new LoggerInformation(callerType), name);
        Loggers.Add(logger);
        return logger;
    }

    /// <summary>
    /// Register a logger.
    /// </summary>
    /// <returns>The configured <see cref="ILogger"/> implementation that will be configured in the <see cref="LogOptions.LoggerBuilder"/> at the <see cref="LogService"/> class</returns>
    public static ILogger RegisterLogger<T>() => RegisterLogger(typeof(T), string.Empty);

    public static ReadOnlyCollection<ILogger> GetLoggers => new ReadOnlyCollection<ILogger>(Loggers.ToList());
    
    /// <summary>
    /// Add a log adapter to the service.
    /// </summary>
    /// <param name="logAdapter">The log adapter based on <see cref="LogAdapterBase"/></param>
    public static void AddLogAdapter(LogAdapterBase logAdapter)
    {
        LogProcessor.LogProcessed += logAdapter.HandleLogEvent;
        Adapters.Add(logAdapter);
    }
    
    /// <summary>
    /// Removes the log adapter from the service.
    /// </summary>
    /// <param name="adapter">The adapter to remove</param>
    /// <returns>True if the adapter is successfully removed otherwise false.</returns>
    public static bool RemoveLogAdapter(LogAdapterBase adapter)
    {
        if (!Adapters.Contains(adapter)) return false;
        LogProcessor.LogProcessed -= adapter.HandleLogEvent;
        return Adapters.Remove(adapter);
    }

    public static ReadOnlyCollection<LogAdapterBase> GetAdapters => new ReadOnlyCollection<LogAdapterBase>(Adapters.ToList());

    internal static bool FilterSeverityLog(LogCapsule capsule)
    {
        if (Options.SeverityFilters.TryGetValue(capsule.Logger.Name, out var namespaceFilter))
            return CanLog(namespaceFilter.Severity, capsule.Severity);
        var filterCapsuleNamespace = Options.SeverityFilters.Where(kvp => capsule.Logger.Name.Contains(kvp.Filter)).Select(v => v).ToList();
        if (filterCapsuleNamespace.Count == 0) return true;
        var filter = filterCapsuleNamespace.FirstOrDefault();
        return CanLog(filter.Severity, capsule.Severity);
    }
}


public readonly struct LoggerInformation
{
    public LoggerInformation(Type? type)
    {
        if (type == null)
            return;
        
        TypeName = type.Name;
        TypeFullName = type.FullName ?? string.Empty;
        TypeNamespace = type.Namespace ?? string.Empty;
        
        var module = type.Module;
        ModuleName = module.Name;
        ModuleScopeName = module.ScopeName;
        ModuleFullyQualifiedName = module.FullyQualifiedName;
        
        var assemblyName = type.Assembly.GetName();
        AssemblyName = assemblyName.Name ?? (type.Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute)).FirstOrDefault() as AssemblyTitleAttribute)?.Title ?? string.Empty;
        AssemblyFullname = assemblyName.FullName;

        if (TypeFullName.IsNullOrEmpty())
            TypeFullName = !TypeNamespace.IsNullOrEmpty() ? $"{TypeNamespace}.{TypeName}" : TypeName;
        if (TypeNamespace.IsNullOrEmpty())
            TypeNamespace = TypeName;
    }

    public string TypeName { get; } = string.Empty;
    public string TypeFullName { get; } = string.Empty;
    public string TypeNamespace { get; } = string.Empty;
    public string AssemblyName { get; } = string.Empty;
    public string AssemblyFullname { get; } = string.Empty;
    public string ModuleName { get; } = string.Empty;
    public string ModuleScopeName { get; } = string.Empty;
    public string ModuleFullyQualifiedName { get; } = string.Empty;
}