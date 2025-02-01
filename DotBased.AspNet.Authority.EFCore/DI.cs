using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotBased.AspNet.Authority.EFCore;

public static class DI
{
    public static IServiceCollection AddAuthorityContext(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
    {
        services.AddDbContextFactory<AuthorityContext>(options);
        return services;
    }
}