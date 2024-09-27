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
    /// <remarks>Use UseBasedServerAuth()!</remarks>
    /// <param name="services">Service collection</param>
    /// <param name="configurationAction">DotBased auth configuration</param>
    public static IServiceCollection AddBasedServerAuth(this IServiceCollection services, Action<BasedAuthConfiguration>? configurationAction = null)
    {
        var Configuration = new BasedAuthConfiguration();
        configurationAction?.Invoke(Configuration);
        
        services.AddSingleton<BasedAuthConfiguration>(Configuration);
        if (Configuration.AuthDataRepositoryType == null)
            throw new ArgumentNullException(nameof(Configuration.AuthDataRepositoryType), $"No '{nameof(IAuthDataRepository)}' configured!");
        services.AddScoped(typeof(IAuthDataRepository), Configuration.AuthDataRepositoryType);
        if (Configuration.SessionStateProviderType == null)
            throw new ArgumentNullException(nameof(Configuration.SessionStateProviderType), $"No '{nameof(ISessionStateProvider)}' configured!");
        services.AddScoped(typeof(ISessionStateProvider), Configuration.SessionStateProviderType);

        services.AddSingleton<AuthDataCache>();
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
        if (authConfig.AuthDataRepositoryType == null)
            throw new NullReferenceException($"{nameof(authConfig.AuthDataRepositoryType)} is null, cannot instantiate an instance of {nameof(IAuthDataRepository)}");
        var dataProvider = (IAuthDataRepository?)Activator.CreateInstance(authConfig.AuthDataRepositoryType);
        if (dataProvider != null) authConfig.SeedData?.Invoke(dataProvider);

        return app;
    }
}