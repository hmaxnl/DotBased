using Serilog;

namespace DotBased.Logging.Serilog;

public static class BasedSerilog
{
    /// <summary>
    /// Default output template with the extra properties that can be used for serilog sinks.
    /// </summary>
    public const string OutputTemplate = "[{Timestamp:HH:mm:ss} - {Caller}->{Assembly}] | {Level:u3}] {Message:lj}{NewLine}{Exception}";
    
    public static LoggerConfiguration UseBasedExtension(this LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration.Enrich.FromLogContext().Enrich.With(new BasedSerilogEnricher());
        return loggerConfiguration;
    }
    
    /// <summary>
    /// The extra properties this implementation adds to serilog
    /// </summary>
    public static class ExtraProperties
    {
        public const string AssemblyProp = "Assembly";
        public const string SourceProp = "Source";
        public const string CallerProp = "Caller";
    }
}