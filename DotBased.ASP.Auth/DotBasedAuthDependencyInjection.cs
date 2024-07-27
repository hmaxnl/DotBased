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
    public static BasedAuthBuilder UseBasedServerAuth(this IServiceCollection services, Action<BasedAuthConfiguration>? configurationAction = null)
    {
        var authBuilder = new BasedAuthBuilder(services, configurationAction);
        return authBuilder;
    }
}