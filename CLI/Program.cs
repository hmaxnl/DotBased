// See https://aka.ms/new-console-template for more information

/*
 * Test CLI application for development of DotBased
 */


using DotBased.Logging.Serilog;
using DotBased.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

var serilogLogger = SetupSerilog();
LogService.AddLogAdapter(new BasedSerilogAdapter(serilogLogger));
var logger = LogService.RegisterLogger(typeof(Program));

logger.Information("Whoah... Hi!");

Console.ReadKey(); // Hold console app open.
return;


ILogger SetupSerilog()
{
    var logConfig = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Console(outputTemplate: BasedSerilog.OutputTemplate);
    return logConfig.CreateLogger();
}