using DotBased.AspNet.Authority.EFCore.Repositories;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotBased.AspNet.Authority.EFCore;

public static class Extensions
{
    public static IServiceCollection AddAuthorityContext(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
    {
        services.AddDbContextFactory<AuthorityContext>(options);
        services.AddScoped<IAttributeRepository, AttributeRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}