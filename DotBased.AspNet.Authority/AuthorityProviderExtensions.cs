using DotBased.AspNet.Authority.Crypto;
using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Options;
using DotBased.AspNet.Authority.Models.Options.Auth;
using DotBased.AspNet.Authority.Services;
using DotBased.AspNet.Authority.Validators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotBased.AspNet.Authority;

public static class AuthorityProviderExtensions
{
    public static AuthorityBuilder AddAuthority(this IServiceCollection services) => AddAuthority(services, _ => { });

    public static AuthorityBuilder AddAuthority(this IServiceCollection services, Action<AuthorityOptions> optionsAction)
    {
        services.AddOptions();
        ArgumentNullException.ThrowIfNull(optionsAction);
        services.Configure(optionsAction);
        
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

    public static AuthenticationBuilder AddAuthorityAuth(this AuthorityBuilder builder) => AddAuthorityAuth(builder, _ => { });

    public static AuthenticationBuilder AddAuthorityAuth(this AuthorityBuilder builder, Action<AuthorityAuthenticationOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(configureOptions);
        builder.Services.Configure(configureOptions);

        builder.Services.AddScoped<IAuthenticationService, AuthorityAuthenticationService>();
        var authBuilder = builder.Services.AddAuthentication(options =>
        {
            
        });
        return authBuilder;
    }

    public static AuthenticationBuilder AddAuthorityCookie(this AuthenticationBuilder builder, string scheme = AuthorityDefaults.Scheme.Cookie.Default)
    {
        builder.AddCookie(options =>
        {
            options.Cookie.Name = AuthorityDefaults.Scheme.Cookie.CookieName;
            options.Cookie.Path = AuthorityDefaults.Paths.Default;
            options.Cookie.Expiration = TimeSpan.FromDays(1);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.LoginPath = AuthorityDefaults.Paths.Login;
            options.LogoutPath = AuthorityDefaults.Paths.Logout;
            options.AccessDeniedPath = AuthorityDefaults.Paths.Forbidden;
            options.SlidingExpiration = true;
            //options.SessionStore
        });
        return builder;
    }

    public static AuthenticationBuilder AddAuthorityToken(this AuthenticationBuilder builder, string scheme = AuthorityDefaults.Scheme.Token.Default)
    {
        
        return builder;
    }

    public static AuthorityBuilder AddAuthorityRepository<TRepository>(this AuthorityBuilder authorityBuilder) where TRepository : class
    {
        return authorityBuilder;
    }

    public static AuthorityBuilder MapAuthorityEndpoints(this AuthorityBuilder builder)
    {
        return builder;
    }
}