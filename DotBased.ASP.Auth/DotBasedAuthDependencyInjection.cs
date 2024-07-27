using DotBased.ASP.Auth.Scheme;
using DotBased.ASP.Auth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DotBased.ASP.Auth;

public static class DotBasedAuthDependencyInjection
{
    /// <summary>
    /// Use the DotBased authentication implementation
    /// </summary>
    /// <remarks>Use the app.UseAuthentication() and app.UseAuthorization()!</remarks>
    /// <param name="services">Service collection</param>
    /// <param name="configurationAction">DotBased auth configuration</param>
    public static IServiceCollection AddBasedServerAuth(this IServiceCollection services, Action<BasedAuthConfiguration>? configurationAction = null)
    {
        /*var authBuilder = new BasedAuthBuilder(services, configurationAction);
        return authBuilder;*/
        var Configuration = new BasedAuthConfiguration();
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
        return services;
    }

    public static WebApplication UseBasedServerAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        
        // Data
        var authConfig = app.Services.GetService<BasedAuthConfiguration>();
        if (authConfig == null)
            throw new NullReferenceException($"{nameof(BasedAuthConfiguration)} is null!");
        if (authConfig.AuthDataProviderType == null)
            throw new NullReferenceException($"{nameof(authConfig.AuthDataProviderType)} is null, cannot instantiate an instance of {nameof(IAuthDataProvider)}");
        var dataProvider = (IAuthDataProvider?)Activator.CreateInstance(authConfig.AuthDataProviderType);
        if (dataProvider != null) authConfig.SeedData?.Invoke(dataProvider);

        return app;
    }
}