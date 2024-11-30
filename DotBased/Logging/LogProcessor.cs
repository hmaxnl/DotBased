using System.Collections.Concurrent;

namespace DotBased.Logging;

/// <summary>
/// Log processor, this class runs a task that send the logs (<see cref="LogCapsule"/>) to all adapters that are registered in the <see cref="LogService"/> class.
/// </summary>
public class LogProcessor : IDisposable
{
    public LogProcessor()
    {
        _canLog = true;
        _capsuleCollection = new BlockingCollection<LogCapsule>();
        IncomingLogHandlerEvent = IncomingLogHandler;
        _processTask = Task.Factory.StartNew(ProcessLog);
    }
    public readonly Action<LogCapsule> IncomingLogHandlerEvent;
    public event EventHandler<LogCapsule>? LogProcessed;
    private bool _canLog;
    private readonly BlockingCollection<LogCapsule> _capsuleCollection;
    private readonly Task _processTask;

    /// <summary>
    /// Stop the LogProcessor
    /// </summary>
    /// <remarks>
    /// The processor cannot be resumed after it is stopped!
    /// </remarks>
    public void Stop()
    {
        _canLog = false;
        _capsuleCollection.CompleteAdding();
        _processTask.Wait();
    }

    public void Dispose()
    {
        Stop();
    }
    
    private void IncomingLogHandler(LogCapsule e)
    {
        if (!_canLog)
            return;
        if (!_capsuleCollection.TryAdd(e))
        {
            _canLog = false;
        }
    }

    private void ProcessLog()
    {
        try
        {
            while (!_capsuleCollection.IsCompleted)
            {
                if (_capsuleCollection.TryTake(out var capsule, Timeout.Infinite))
                {
                    if (!LogService.CanLog(LogService.Options.Severity, capsule.Severity))
                        continue;
                    if (LogService.FilterSeverityLog(capsule))
                        LogProcessed?.Invoke(this, capsule);
                }
            }
        }
        catch (InvalidOperationException)
        { }
        catch (Exception e)
        {
            // Write exception to the output
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==================================================================================");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[{DateTime.Now}] ");
            Console.ForegroundColor = oldColor;
            Console.WriteLine(
                $"[{nameof(LogProcessor)} (DotBased)] Log processor thread failed! No logs are being processed!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==================================================================================");
            Console.ForegroundColor = oldColor;
            //TODO: Write to disk.
        }
    }
}