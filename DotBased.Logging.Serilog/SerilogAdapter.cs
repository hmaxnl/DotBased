using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Serilog.Events;
using Serilog.Parsing;

namespace DotBased.Logging.Serilog;

public class SerilogAdapter : LogAdapterBase
{
    public SerilogAdapter(global::Serilog.ILogger serilogLogger) : base("Serilog adapter")
    {
        _serilogLogger = serilogLogger;
        _messageTemplateParser = new MessageTemplateParser();
    }

    public const string SampleTemplate = "[{Timestamp:HH:mm:ss} - {Caller} -> {Source}] | {Level:u3}] {Message:lj}{NewLine}{Exception}";
    
    private readonly global::Serilog.ILogger _serilogLogger;
    private readonly MessageTemplateParser _messageTemplateParser;

    public override void HandleLog(object? sender, LogCapsule? capsule)
    {
        if (capsule == null)
            return;
        var baseLogger = capsule.Logger as Logger;
        var logger = _serilogLogger
            .ForContext("Source", baseLogger?.Source.AssemblyName ?? "Static")
            .ForContext("Caller", baseLogger?.Identifier);

        var template = _messageTemplateParser.Parse(capsule.Message);
        IEnumerable<LogEventProperty>? properties = null;
        if (capsule.Parameters != null && capsule.Parameters.Length != 0)
        {
            var tokenList = template.Tokens.OfType<PropertyToken>().ToList();
            properties = capsule.Parameters.Zip(tokenList, (p, t) => new LogEventProperty(t.PropertyName, new ScalarValue(p)));
        }
        switch (capsule.Severity)
        {
            case LogSeverity.Trace:
            default:
                logger.Write(new LogEvent(capsule.TimeStamp, LogEventLevel.Verbose, null, template, properties ?? ArraySegment<LogEventProperty>.Empty, ActivityTraceId.CreateRandom(), ActivitySpanId.CreateRandom()));
                break;
            case LogSeverity.Debug:
                logger.Write(new LogEvent(capsule.TimeStamp, LogEventLevel.Debug, null, template, properties ?? ArraySegment<LogEventProperty>.Empty, ActivityTraceId.CreateRandom(), ActivitySpanId.CreateRandom()));
                break;
            case LogSeverity.Info:
                logger.Write(new LogEvent(capsule.TimeStamp, LogEventLevel.Information, null, template, properties ?? ArraySegment<LogEventProperty>.Empty, ActivityTraceId.CreateRandom(), ActivitySpanId.CreateRandom()));
                break;
            case LogSeverity.Warning:
                logger.Write(new LogEvent(capsule.TimeStamp, LogEventLevel.Warning, null, template, properties ?? ArraySegment<LogEventProperty>.Empty, ActivityTraceId.CreateRandom(), ActivitySpanId.CreateRandom()));
                break;
            case LogSeverity.Error:
                logger.Write(new LogEvent(capsule.TimeStamp, LogEventLevel.Error, capsule.Exception, template, properties ?? ArraySegment<LogEventProperty>.Empty, ActivityTraceId.CreateRandom(), ActivitySpanId.CreateRandom()));
                break;
            case LogSeverity.Fatal:
                logger.Write(new LogEvent(capsule.TimeStamp, LogEventLevel.Fatal, capsule.Exception, template, properties ?? ArraySegment<LogEventProperty>.Empty, ActivityTraceId.CreateRandom(), ActivitySpanId.CreateRandom()));
                break;
        }
    }
}