using DotBased.AspNet.Authority;
using DotBased.AspNet.Authority.EFCore;
using DotBased.AspNet.Authority.Models.Options.Auth;
using DotBased.Logging;
using DotBased.Logging.MEL;
using DotBased.Logging.Serilog;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TestWebApi;
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
builder.Logging.AddDotBasedLoggerProvider(LogService.Options);

builder.Services.AddControllers();

builder.Services.AddAuthority().AddAuthorityContext(options =>
{
    options.UseSqlite("Data Source=dev-authority.db", c => c.MigrationsAssembly("TestWebApi"));
}).AddAuthorityAuth(options =>
{
    options.DefaultScheme = AuthorityDefaults.Scheme.Authority.AuthenticationScheme;
    //TODO: Auto detect auth and session store schemes?
    options.SchemeMap = [
        new SchemeInfo
        {
            Scheme = AuthorityDefaults.Scheme.Authority.AuthenticationScheme,
            Identifier = "Authority password login",
            Type = SchemeType.Authentication,
            AuthenticationType = "Password"
        },
        new SchemeInfo
        {
            Scheme = "OIDC",
            Identifier = "Authentik OIDC login",
            Type = SchemeType.Authentication,
            AuthenticationType = "OpenIdConnect"
        },
        new SchemeInfo
        {
            Scheme = AuthorityDefaults.Scheme.Cookie.Default,
            Identifier = "Cookie session",
            Type = SchemeType.SessionStore
        },
        new SchemeInfo
        {
            Scheme = AuthorityDefaults.Scheme.Token.Default,
            Identifier = "Session token",
            Type = SchemeType.SessionStore
        }
    ];
})
.AddAuthorityLoginScheme()
.AddAuthorityCookie()
.AddAuthorityToken();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await SeedAuthorityData.InitializeData(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
return;

ILogger SetupSerilog()
{
    var logConfig = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Console(outputTemplate: BasedSerilog.OutputTemplate);
    return logConfig.CreateLogger();
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}