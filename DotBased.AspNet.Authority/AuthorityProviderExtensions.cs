using DotBased.AspNet.Authority.Crypto;
using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Models.Options;
using DotBased.AspNet.Authority.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotBased.AspNet.Authority;

public static class AuthorityProviderExtensions
{
    public static AuthorityBuilder AddAuthority(this IServiceCollection services, Action<AuthorityOptions>? optionsAction = null)
        => services.AddAuthority<AuthorityUser, AuthorityGroup, AuthorityRole>(optionsAction);

    public static AuthorityBuilder AddAuthority<TUser, TGroup, TRole>(this IServiceCollection services, Action<AuthorityOptions>? optionsAction = null)
        where TUser : class where TGroup : class where TRole : class
    {
        if (optionsAction != null)
        {
            services.AddOptions();
            services.Configure<AuthorityOptions>(optionsAction);
        }
        
        services.TryAddScoped<ICryptographer, Cryptographer>();
        services.TryAddScoped<IPasswordHasher, PasswordHasher>();
        services.TryAddScoped<IPasswordValidator<TUser>, PasswordOptionsValidator<TUser>>();
        services.TryAddScoped<IPasswordValidator<TUser>, PasswordEqualsValidator<TUser>>();
        services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
        /*services.TryAddScoped<IEmailVerifier, EmailVerifier>();
        services.TryAddScoped<IPhoneNumberVerifier, PhoneNumberVerifier>();
        services.TryAddScoped<IUserVerifier, UserVerifier>();*/
        services.TryAddScoped<AuthorityManager>();
        services.TryAddScoped<AuthorityUserManager<TUser>>();
        services.TryAddScoped<AuthorityGroupManager<TGroup>>();
        services.TryAddScoped<AuthorityRoleManager<TRole>>();
        return new AuthorityBuilder(services);
    }

    public static AuthorityBuilder AddAuthorityRepository<TRepository>(this AuthorityBuilder authorityBuilder) where TRepository : class
    {
        return authorityBuilder;
    }

    public static AuthorityBuilder MapAuthorityEndpoints(this AuthorityBuilder builder)
    {
        return builder;
    }

    private static Type GetBaseGenericArgumentType<TModel>(Type baseType)
    {
        var userGenericBaseTypeDefinition = typeof(TModel).BaseType?.GetGenericTypeDefinition();
        if (userGenericBaseTypeDefinition != null && userGenericBaseTypeDefinition == baseType)
        {
            var userBaseGenericArguments = userGenericBaseTypeDefinition.GetGenericArguments();
            if (userBaseGenericArguments.Length <= 0)
            {
                throw new ArgumentException("Base implementation does not have the required generic argument.", nameof(TModel));
            }

            return userBaseGenericArguments[0];
        }
        throw new ArgumentException($"Given object {typeof(TModel).Name} does not have the base implementation type of: {baseType.Name}", nameof(TModel));
    }
}