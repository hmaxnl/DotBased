using DotBased.AspNet.Authority.EFCore.Repositories;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotBased.AspNet.Authority.EFCore;

public static class Extensions
{
    public static AuthorityBuilder AddAuthorityContext(this AuthorityBuilder builder, Action<DbContextOptionsBuilder> options)
    {
        builder.Services.AddDbContextFactory<AuthorityContext>(options);
        builder.Services.AddScoped<IAttributeRepository, AttributeRepository>();
        builder.Services.AddScoped<IGroupRepository, GroupRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        return builder;
    }
}