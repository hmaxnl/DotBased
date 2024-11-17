using DotBased.Logging;
using DotBased.Logging.MEL;
using DotBased.Logging.Serilog;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

LogService.Initialize(options =>
{
    options
        .AddSeverityFilter("Program", LogSeverity.Verbose);
});

var serilogLogger = SetupSerilog();
LogService.AddLogAdapter(new BasedSerilogAdapter(serilogLogger));

builder.Logging.ClearProviders();
builder.Logging.AddDotBased(LogService.Options);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

ILogger SetupSerilog()
{
    var logConfig = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Console(outputTemplate: BasedSerilog.OutputTemplate);
    return logConfig.CreateLogger();
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}