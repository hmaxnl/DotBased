using Microsoft.Extensions.Logging;

namespace DotBased.Logging.MEL;

public class BasedLoggerProvider : ILoggerProvider
{
    public BasedLoggerProvider(LogOptions options)
    {
        Options = options;
    }

    private readonly LogOptions Options;
    
    public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
    {
        return new BasedLogger(Options.LoggerBuilder.Invoke(new LoggerInformation(typeof(BasedLoggerProvider)), categoryName));
    }
    
    public void Dispose()
    {
        
    }
}