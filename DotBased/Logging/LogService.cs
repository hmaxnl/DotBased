using System.Collections.ObjectModel;
using System.Reflection;

namespace DotBased.Logging;

/// <summary>
/// Main log service class, handles the loggers, log processor and adapters.
/// </summary>
public static class LogService
{
    // TODO: Future: add middlewares and changeable log processor
    static LogService()
    {
        Options = new LogOptions();
        LoggerSendEvent = LogProcessor.IncommingLogHandlerEvent;
    }
    public static bool ShouldLog(LogSeverity maxSeverity, LogSeverity severity) => maxSeverity <= severity;
    public static LogOptions Options { get; private set; }
    public static LogProcessor LogProcessor { get; private set; } = new LogProcessor();
    
    private static HashSet<LogAdapterBase> Adapters { get; } = new HashSet<LogAdapterBase>();
    private static HashSet<LoggerBase> Loggers { get; } = new HashSet<LoggerBase>();

    /// <summary>
    /// Action for internal communication between loggers and processor
    /// </summary>
    private static readonly Action<LogCapsule> LoggerSendEvent;

    /// <summary>
    /// Register a logger that will be used in a class and will live as long as the class.
    /// This will get the calling assembly and will pass that through ther log adapters.
    /// </summary>
    /// <example>
    /// <code>
    /// public class Program
    /// {
    ///     public Program
    ///     {
    ///         logger = LogService.RegisterLogger(nameof(Program));
    ///     }
    ///     private ILogger logger;
    /// }
    /// </code>
    /// </example>
    /// <param name="callerType">The type that called the function</param>
    /// <returns>The configured <see cref="ILogger"/> implementation that will be configuered in the <see cref="LogOptions.LoggerBuilder"/> at the <see cref="LogService"/> class</returns>
    public static ILogger RegisterLogger(Type callerType)
    {
        var logger = Options.LoggerBuilder.Invoke(new CallerInformation(callerType), LoggerSendEvent);
        Loggers.Add(logger);
        return logger;
    }

    public static bool UnregisterLogger(LoggerBase logger) => Loggers.Remove(logger);

    public static ReadOnlyCollection<LoggerBase> GetLoggers => new ReadOnlyCollection<LoggerBase>(Loggers.ToList());
    
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
    /// <returns>True if the adapter is succesfully removed otherwise false.</returns>
    public static bool RemoveLogAdapter(LogAdapterBase adapter)
    {
        if (!Adapters.Contains(adapter)) return false;
        LogProcessor.LogProcessed -= adapter.HandleLogEvent;
        return Adapters.Remove(adapter);
    }

    public static ReadOnlyCollection<LogAdapterBase> GetAdapters =>
        new ReadOnlyCollection<LogAdapterBase>(Adapters.ToList());
}


public readonly struct CallerInformation
{
    public CallerInformation(Type type)
    {
        Name = type.Name;
        Source = type.FullName ?? type.GUID.ToString();
        Namespace = type.Namespace ?? string.Empty;
        SourceAssembly = type.Assembly;
        
        var asmName = SourceAssembly.GetName();
        AssemblyName = asmName.Name ?? "Unknown";
        AssemblyFullname = asmName.FullName;
    }
    public string Name { get; }
    public string Source { get; }
    public string Namespace { get; }
    public Assembly SourceAssembly { get; }
    public string AssemblyName { get; }
    public string AssemblyFullname { get; }
}