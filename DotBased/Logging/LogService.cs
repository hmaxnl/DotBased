using System.Reflection;

namespace DotBased.Logging;

/// <summary>
/// Main log service class, handles the loggers, log processor and adapters.
/// </summary>
public static class LogService
{
    static LogService()
    {
        Options = new LogOptions();
        _loggerSendEvent = LogProcessor.IncommingLogHandlerEvent;
    }
    public static bool ShouldLog(LogSeverity maxSeverity, LogSeverity severity) => maxSeverity <= severity;
    public static LogOptions Options { get; private set; }
    public static LogProcessor LogProcessor { get; private set; } = new LogProcessor();
    
    private static HashSet<LogAdapterBase> Adapters { get; } = new HashSet<LogAdapterBase>();
    private static HashSet<ILogger> Loggers { get; } = new HashSet<ILogger>();

    /// <summary>
    /// Internal communication between loggers and processor
    /// </summary>
    private static Action<LogCapsule> _loggerSendEvent;

    public static void AddLogAdapter(LogAdapterBase logAdapter)
    {
        LogProcessor.LogProcessed += logAdapter.HandleLogEvent;
        Adapters.Add(logAdapter);
    }

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
    /// <param name="identifier">The identifier name of the logger, this will be passed to the log adapter as the source.</param>
    /// <returns>The configured <see cref="ILogger"/> implementation that will be configuered in the <see cref="LogOptions.LoggerBuilder"/> at the <see cref="LogService"/> class</returns>
    public static ILogger RegisterLogger(string identifier)
    {
        var asm = Assembly.GetCallingAssembly();
        var logger = Options.LoggerBuilder.Invoke(identifier, CallingSource.LoadFromAsm(asm), _loggerSendEvent);
        Loggers.Add(logger);
        return logger;
    }
}

/// <summary>
/// Data struct for holding calling source information.
/// </summary>
public struct CallingSource
{
    private CallingSource(Assembly asm)
    {
        AssemblySource = asm;
        var asmName = AssemblySource.GetName();
        AssemblyName = asmName.Name ?? "Unknown";
        AssemblyFullName = asmName.FullName;
    }
    public static CallingSource LoadFromAsm(Assembly asm) => new CallingSource(asm);

    public Assembly AssemblySource { get; }
    public string AssemblyName { get; }
    public string AssemblyFullName { get; set; }
}