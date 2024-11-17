using Serilog;

namespace DotBased.Logging.Serilog;

public static class BasedSerilog
{
    /// <summary>
    /// Default output template with the extra properties that can be used for serilog sinks.
    /// </summary>
    public const string OutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3} - {LoggerName}]{NewLine} {Message:lj}{NewLine}{Exception}";
    
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
        public const string LoggerName = "LoggerName";
        public const string AssemblyProp = "Assembly";
        public const string FullNameProp = "FullName";
        public const string NamespaceProp = "Namespace";
        public const string CallerProp = "Caller";
    }
}