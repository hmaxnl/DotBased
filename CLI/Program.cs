// See https://aka.ms/new-console-template for more information

using DotBased.Logging.Serilog;
using DotBased.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

var serilogLogger = SetupSerilog();

LogService.AddLogAdapter(new SerilogAdapter(serilogLogger));

var logger = LogService.RegisterLogger(nameof(Program));

logger.Trace("Test TRACE log! {StringValue} {AnotherValue}", "WOW", "W0W");
logger.Debug("Test DEBUG log! {IntVal}", 69);
logger.Information("Test INFO log! {DoubVal}", 4.20);
logger.Warning("Test WARNING log! {StrVal} {IntVAl} {StrChar}", "Over", 9000, '!');
logger.Error(new NullReferenceException("Test exception"),"Test ERROR log!");
logger.Fatal(new NullReferenceException("Test exception"),"Test FATAL log!");

Console.ReadKey();
return;


ILogger SetupSerilog()
{
    var logConfig = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Console(outputTemplate: SerilogAdapter.SampleTemplate);
    return logConfig.CreateLogger();
}