using System.Reflection;

namespace DotBased.Logging;

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

    public static ILogger RegisterLogger(string identifier)
    {
        var asm = Assembly.GetCallingAssembly();
        var logger = new Logger(identifier, CallingSource.LoadFromAsm(asm),  ref _loggerSendEvent);
        Loggers.Add(logger);
        return logger;
    }
}

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