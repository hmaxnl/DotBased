using Microsoft.Extensions.Logging;

namespace DotBased.Logging.MEL;

public static class LoggerBuilderExtensions
{
    public static ILoggingBuilder AddDotBased(this ILoggingBuilder builder, LogOptions options)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));
        builder.AddProvider(new BasedLoggerProvider(options));
        return builder;
    }
}