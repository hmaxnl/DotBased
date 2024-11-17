using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Serilog.Events;
using Serilog.Parsing;

namespace DotBased.Logging.Serilog;

public class BasedSerilogAdapter(global::Serilog.ILogger serilogLogger) : LogAdapterBase("Serilog adapter")
{
    private readonly MessageTemplateParser _messageTemplateParser = new();

    public override void HandleLog(object? processor, LogCapsule? capsule)
    {
        if (capsule == null)
            return;
        var logger = serilogLogger
            .ForContext(BasedSerilog.ExtraProperties.LoggerName, capsule.Logger.Name)
            .ForContext(BasedSerilog.ExtraProperties.AssemblyProp, capsule.Logger.LoggerInformation.AssemblyName)
            .ForContext(BasedSerilog.ExtraProperties.FullNameProp, capsule.Logger.LoggerInformation.TypeFullName)
            .ForContext(BasedSerilog.ExtraProperties.NamespaceProp, capsule.Logger.LoggerInformation.TypeNamespace)
            .ForContext(BasedSerilog.ExtraProperties.CallerProp, capsule.Logger.LoggerInformation.TypeName);

        var template = _messageTemplateParser.Parse(capsule.Message);
        IEnumerable<LogEventProperty>? properties = null;
        if (capsule.Parameters != null && capsule.Parameters.Length != 0)
        {
            var tokenList = template.Tokens.OfType<PropertyToken>().ToList();
            properties = capsule.Parameters.Zip(tokenList, (p, t) => new LogEventProperty(t.PropertyName, new ScalarValue(p)));
        }
        switch (capsule.Severity)
        {
            case LogSeverity.Verbose:
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