using Serilog.Core;
using Serilog.Events;

namespace DotBased.Logging.Serilog;

public class BasedSerilogEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var asmPropValue = "ASM";
        var sourcePropValue = "Unknown";
        if (logEvent.Properties.TryGetValue("SourceContext", out var sourceContextValue))
            asmPropValue = sourceContextValue.ToString().Replace("\"", "");
        if (logEvent.Properties.TryGetValue("Application", out var appValue))
            sourcePropValue = appValue.ToString().Replace("\"", "");

        var assemblyProperty = propertyFactory.CreateProperty(BasedSerilog.ExtraProperties.AssemblyProp, asmPropValue);
        var sourceProperty = propertyFactory.CreateProperty(BasedSerilog.ExtraProperties.FullNameProp, sourcePropValue);
        var callerProperty = propertyFactory.CreateProperty(BasedSerilog.ExtraProperties.CallerProp, sourcePropValue);
        
        logEvent.AddPropertyIfAbsent(assemblyProperty);
        logEvent.AddPropertyIfAbsent(sourceProperty);
        logEvent.AddPropertyIfAbsent(callerProperty);
    }
}