// See https://aka.ms/new-console-template for more information

/*
 * Test CLI application for development of DotBased
 */


using DotBased.Logging.Serilog;
using DotBased.Logging;
using DotBased.Utilities;
using Serilog;
using ILogger = Serilog.ILogger;

LogService.Initialize(options =>
{
    options
        .AddSeverityFilter("Program", LogSeverity.Verbose)
        .AddSeverityFilter("DotBased.dll", LogSeverity.Verbose);
});

var serilogLogger = SetupSerilog();
LogService.AddLogAdapter(new BasedSerilogAdapter(serilogLogger));
var logger = LogService.RegisterLogger<Program>();

logger.Information("Whoah... Hi!, {Param}", "Test!");
var cult = Culture.GetSystemCultures();

Console.ReadKey(); // Hold console app open.
return;


ILogger SetupSerilog()
{
    var logConfig = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Console(outputTemplate: BasedSerilog.OutputTemplate);
    return logConfig.CreateLogger();
}