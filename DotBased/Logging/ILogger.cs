namespace DotBased.Logging;

/// <summary>
/// ILogger interface used by <see cref="LoggerBase"/>
/// </summary>
public interface ILogger
{
    public void Verbose(string message, params object?[]? parameters);
    
    public void Trace(string message, params object?[]? parameters);

    public void Debug(string message, params object?[]? parameters);

    public void Information(string message, params object?[]? parameters);

    public void Warning(string message, params object?[]? parameters);

    public void Error(Exception exception, string message, params object?[]? parameters);

    public void Fatal(Exception exception, string message, params object?[]? parameters);
}