namespace DotBased.Logging;

/// <summary>
/// Log processor, this class runs a task that send the logs (<see cref="LogCapsule"/>) to all adapters that are registered in the <see cref="LogService"/> class.
/// </summary>
public class LogProcessor : IDisposable
{
    public LogProcessor()
    {
        _processorQueue = new Queue<LogCapsule>();
        IncomingLogHandlerEvent = IncomingLogHandler;
        _processorThread = new Thread(ProcessLog)
        {
            IsBackground = true,
            Name = "Log processor thread (DotBased)"
        };
        _processorThread.Start();
    }
    public readonly Action<LogCapsule> IncomingLogHandlerEvent;
    public event EventHandler<LogCapsule>? LogProcessed;
    private readonly Queue<LogCapsule> _processorQueue;
    private readonly Thread _processorThread;
    private readonly ManualResetEvent _threadSuspendEvent = new ManualResetEvent(false);
    private readonly ManualResetEvent _threadShutdownEvent = new ManualResetEvent(false);

    /// <summary>
    /// Stop the LogProcessor
    /// </summary>
    /// <remarks>
    /// The processor cannot be resumed after it is stopped!
    /// </remarks>
    public void Stop()
    {
        _threadShutdownEvent.Set();
        _threadSuspendEvent.Set();
        _processorThread.Join();
    }

    public void Dispose()
    {
        Stop();
    }
    
    private void IncomingLogHandler(LogCapsule e)
    {
        _processorQueue.Enqueue(e);
        // Check if the thread is running, if not wake up the thread.
        if (!_threadSuspendEvent.WaitOne(0))
            _threadSuspendEvent.Set();
    }

    private void ProcessLog()
    {
        try
        {
            while (true)
            {
                _threadSuspendEvent.WaitOne(Timeout.Infinite);
            
                if (_threadShutdownEvent.WaitOne(0))
                    break;

                if (_processorQueue.Count != 0)
                {
                    var capsule = _processorQueue.Dequeue();
                    if (!LogService.CanLog(LogService.Options.Severity, capsule.Severity))
                        continue;
                    if (LogService.FilterSeverityLog(capsule))
                        LogProcessed?.Invoke(this, capsule);
                }
                else
                    _threadSuspendEvent.Reset();
            }
        }
        catch (Exception e)
        {
            // Write exception to the output
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==================================================================================");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[{DateTime.Now}] ");
            Console.ForegroundColor = oldColor;
            Console.WriteLine($"[{nameof(LogProcessor)} (DotBased)] Log processor thread failed! No logs are being processed!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==================================================================================");
            Console.ForegroundColor = oldColor;
            //TODO: Write to disk.
        }
    }
}