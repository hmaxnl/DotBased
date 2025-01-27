using DotBased.AspNet.Authority.Crypto;
using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Options;
using DotBased.AspNet.Authority.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotBased.AspNet.Authority;

public static class AuthorityProviderExtensions
{
    public static AuthorityBuilder AddAuthority(this IServiceCollection services, Action<AuthorityOptions>? optionsAction = null)
    {
        if (optionsAction != null)
        {
            services.AddOptions();
            services.Configure<AuthorityOptions>(optionsAction);
        }
        
        services.TryAddScoped<ICryptographer, Cryptographer>();
        services.TryAddScoped<IPasswordHasher, PasswordHasher>();
        services.TryAddScoped<IPasswordValidator, PasswordOptionsValidator>();
        services.TryAddScoped<IPasswordValidator, PasswordEqualsValidator>();
        services.TryAddScoped<IUserValidator, UserValidator>();
        /*services.TryAddScoped<IEmailVerifier, EmailVerifier>();
        services.TryAddScoped<IPhoneNumberVerifier, PhoneNumberVerifier>();
        services.TryAddScoped<IUserVerifier, UserVerifier>();*/
        services.TryAddScoped<AuthorityManager>();
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