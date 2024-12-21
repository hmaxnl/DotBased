using DotBased.AspNet.Authority.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DotBased.AspNet.Authority;

public static class AuthorityProviderExtensions
{
    public static AuthorityBuilder AddAuthorityProvider<TModel>(this IServiceCollection services) where TModel : class
    {
        return new AuthorityBuilder(services);
    }

    public static AuthorityBuilder AddAuthorityStore<TStore>(this AuthorityBuilder authorityBuilder) where TStore : IAuthorityRepository
    {
        return authorityBuilder;
    }
}