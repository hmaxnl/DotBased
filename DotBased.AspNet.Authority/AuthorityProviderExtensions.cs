using DotBased.AspNet.Authority.Interfaces;
using DotBased.AspNet.Authority.Models.Options;
using Microsoft.Extensions.DependencyInjection;

namespace DotBased.AspNet.Authority;

public static class AuthorityProviderExtensions
{
    public static AuthorityBuilder AddAuthorityProvider<TModel>(this IServiceCollection services, Action<AuthorityOptions> optionsAction) where TModel : class
    {
        services.AddOptions();
        // Configure required classes, services, etc.
        services.Configure<AuthorityOptions>(optionsAction);
        return new AuthorityBuilder(services);
    }

    public static AuthorityBuilder AddAuthorityStore<TStore>(this AuthorityBuilder authorityBuilder) where TStore : IAuthorityRepository
    {
        return authorityBuilder;
    }

    public static AuthorityBuilder MapAuthorityEndpoints(this AuthorityBuilder builder)
    {
        return builder;
    }
}