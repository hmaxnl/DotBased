using DotBased.ASP.Authentication.Configuration;
using DotBased.Logging;
using Microsoft.Extensions.Options;

namespace DotBased.ASP.Authentication;

public class SecurityManager
{
    public SecurityManager(IServiceProvider services, IOptions<AuthenticationConfiguration>? config)
    {
        _services = services;
        Configuration = config?.Value ?? new AuthenticationConfiguration();
    }
    private ILogger _logger = LogService.RegisterLogger<SecurityManager>();
    private IServiceProvider _services;
    public AuthenticationConfiguration Configuration { get; set; }
}