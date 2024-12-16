using Microsoft.Extensions.DependencyInjection;

namespace DotBased.AspNet.Auth;

public static class BasedAuthExtensions
{
    public static IServiceCollection AddBasedAuthentication(this IServiceCollection services)
    {
        return services;
    }
}