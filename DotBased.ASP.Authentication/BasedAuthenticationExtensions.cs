using DotBased.ASP.Authentication.Configuration;
using DotBased.ASP.Authentication.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DotBased.ASP.Authentication;

public static class BasedAuthenticationExtensions
{
    public static BasedAuthenticationBuilder AddBasedAuthentication(this IServiceCollection services, Action<AuthenticationConfiguration>? configurationAction)
    {
        /*
         * Add services
         * - Validators
         * - Managers
         * - Services
         */
        if (configurationAction != null)
        {
            services.Configure(configurationAction);
        }

        return new BasedAuthenticationBuilder(typeof(BasedAuthenticationBuilder));
    }

    public static BasedAuthenticationBuilder AddRepository<TRepository>(this BasedAuthenticationBuilder builder)
    {
        return builder;
    }

    public static BasedAuthenticationBuilder SeedData<TRepository>(this BasedAuthenticationBuilder builder, Action<TRepository> seeder) where TRepository : RepositoryBase
    {
        return builder;
    }
}