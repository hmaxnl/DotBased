using DotBased.ASP.Auth.Scheme;
using DotBased.ASP.Auth.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DotBased.ASP.Auth;

public class BasedAuthBuilder
{
    public BasedAuthBuilder(IServiceCollection services, Action<BasedAuthConfiguration>? configurationAction = null)
    {
        _services = services;
        Configuration = new BasedAuthConfiguration();
        configurationAction?.Invoke(Configuration);
        
        services.AddSingleton<BasedAuthConfiguration>(Configuration);
        if (Configuration.AuthDataProviderType == null)
            throw new ArgumentNullException(nameof(Configuration.AuthDataProviderType), $"No '{nameof(IAuthDataProvider)}' configured!");
        services.AddScoped(typeof(IAuthDataProvider), Configuration.AuthDataProviderType);
        if (Configuration.SessionStateProviderType == null)
            throw new ArgumentNullException(nameof(Configuration.SessionStateProviderType), $"No '{nameof(ISessionStateProvider)}' configured!");
        services.AddScoped(typeof(ISessionStateProvider), Configuration.SessionStateProviderType);
        
        services.AddScoped<AuthService>();
        
        services.AddScoped<AuthenticationStateProvider, BasedServerAuthenticationStateProvider>();
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = BasedAuthenticationHandler.AuthenticationScheme;
        }).AddScheme<BasedAuthenticationHandlerOptions, BasedAuthenticationHandler>(BasedAuthenticationHandler.AuthenticationScheme, null);
        services.AddAuthorization();
        services.AddCascadingAuthenticationState();
    }
    public BasedAuthConfiguration Configuration { get; }
    private readonly IServiceCollection _services;
}