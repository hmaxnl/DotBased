namespace DotBased.Logging;

/// <summary>
/// The ILogger interface for creating loggers that can be used by the <see cref="LogService"/>
/// </summary>
public interface ILogger
{
    public void Trace(string message, params object?[]? parameters);

    public void Debug(string message, params object?[]? parameters);

    public void Information(string message, params object?[]? parameters);

    public void Warning(string message, params object?[]? parameters);

    public void Error(Exception exception, string message, params object?[]? parameters);

    public void Fatal(Exception exception, string message, params object?[]? parameters);
}