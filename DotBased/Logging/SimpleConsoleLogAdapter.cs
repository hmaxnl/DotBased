using System.Text;

namespace DotBased.Logging;

public class SimpleConsoleLogAdapter : LogAdapterBase
{
    public SimpleConsoleLogAdapter(string adapterName) : base(adapterName)
    {
        Console.OutputEncoding = Encoding.UTF8;
    }
    
    private readonly ConsoleColor _defaultColor = Console.ForegroundColor;
    private const ConsoleColor TimestampColor = ConsoleColor.DarkBlue;
    private const ConsoleColor BrackedColor = ConsoleColor.Gray;
    private const ConsoleColor MessageColor = ConsoleColor.DarkGray;
    private ConsoleColor _severityColor = ConsoleColor.Cyan;

    public override void HandleLog(object? sender, LogCapsule? capsule)
    {
        if (capsule == null) return;
        
        Console.ForegroundColor = BrackedColor;
        Console.Write("[");
        Console.ForegroundColor = TimestampColor;
        Console.Write($"{capsule.TimeStamp}");
        Console.ForegroundColor = BrackedColor;
        Console.Write("] ");
        
        _severityColor = capsule.Severity.ToConsoleColor();
        WriteSeverity(capsule.Severity);

        if (capsule.Severity is LogSeverity.Error or LogSeverity.Fatal)
            WriteException(capsule);
        else
            WriteMessage(capsule);
        Console.ForegroundColor = _defaultColor;
    }

    private void WriteSeverity(LogSeverity severity)
    {
        Console.ForegroundColor = BrackedColor;
        Console.Write("[");
        Console.ForegroundColor = _severityColor;
        Console.Write($"{severity}");
        Console.ForegroundColor = BrackedColor;
        Console.Write("] ");
    }

    private void WriteMessage(LogCapsule capsule)
    {
        if (capsule.Parameters == null || !capsule.Parameters.Any())
        {
            Console.ForegroundColor = MessageColor;
            Console.WriteLine($"{capsule.Message}");
        }
        else
        {
            
        }
    }

    private void WriteException(LogCapsule capsule)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\u23F7");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("==============================================================================================");
        WriteMessage(capsule);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(capsule.Exception);
        Console.WriteLine("==============================================================================================");
        Console.ForegroundColor = _defaultColor;
    }
}

public static class LogSeverityExt
{
    public static ConsoleColor ToConsoleColor(this LogSeverity severity)
    {
        var color = severity switch
        {
            LogSeverity.Trace or LogSeverity.Info => ConsoleColor.White,
            LogSeverity.Debug => ConsoleColor.Magenta,
            LogSeverity.Warning => ConsoleColor.Yellow,
            LogSeverity.Error => ConsoleColor.Red,
            LogSeverity.Fatal => ConsoleColor.DarkRed,
            _ => ConsoleColor.White
        };
        return color;
    }
}