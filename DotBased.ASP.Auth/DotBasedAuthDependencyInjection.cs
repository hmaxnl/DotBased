using DotBased.ASP.Auth.Scheme;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DotBased.ASP.Auth;

public static class DotBasedAuthDependencyInjection
{
    /// <summary>
    /// Use the DotBased authentication implementation
    /// </summary>
    /// <remarks>Use the app.UseAuthentication() and app.UseAuthorization()!</remarks>
    /// <param name="services">Service colllection</param>
    /// <param name="configurationAction">DotBased auth configuration</param>
    public static void UseBasedAuth(this IServiceCollection services, Action<BasedAuthConfiguration>? configurationAction = null)
    {
        var config = new BasedAuthConfiguration();
        configurationAction?.Invoke(config);
        services.AddSingleton<BasedAuthConfiguration>(config);
        
        services.AddScoped<AuthenticationStateProvider, BasedAuthenticationStateProvider>();
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = BasedAuthenticationHandler.AuthenticationScheme;
        }).AddScheme<BasedAuthenticationHandlerOptions, BasedAuthenticationHandler>(BasedAuthenticationHandler.AuthenticationScheme, null);
        services.AddAuthorization();
    }
}