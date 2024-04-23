namespace DotBased.Logging;

public class LogProcessor : IDisposable
{
    public LogProcessor()
    {
        _processorQueue = new Queue<LogCapsule>();
        IncommingLogHandlerEvent = IncommingLogHandler;
        _processorThread = new Thread(ProcessLog)
        {
            IsBackground = true,
            Name = "Log processor thread (DotBased)"
        };
        _processorThread.Start();
    }
    public readonly Action<LogCapsule> IncommingLogHandlerEvent;
    public event EventHandler<LogCapsule>? LogProcessed;
    private readonly Queue<LogCapsule> _processorQueue;
    private readonly Thread _processorThread;
    private readonly ManualResetEvent _threadSuspendEvent = new ManualResetEvent(false);
    private readonly ManualResetEvent _threadShutdownEvent = new ManualResetEvent(false);

    /// <summary>
    /// Stop the LogProcessor, the processor cannot be resumed after stopped!
    /// </summary>
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
    
    private void IncommingLogHandler(LogCapsule e)
    {
        _processorQueue.Enqueue(e);
        // Check is the thread is running, if not wake up the thread.
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

                if (_processorQueue.Any())
                {
                    var capsule = _processorQueue.Dequeue();
                    if (LogService.ShouldLog(LogService.Options.Severity, capsule.Severity))
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
            //TODO: Write info to disk.
        }
    }
}