using DotBased.AspNet.Authority.Crypto;
using DotBased.AspNet.Authority.Handlers;
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

    public static AuthenticationBuilder AddAuthorityAuth(this AuthorityBuilder builder, Action<AuthorityAuthenticationOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(configureOptions);
        builder.Services.Configure(configureOptions);

        builder.Services.AddScoped<IAuthenticationService, AuthorityAuthenticationService>();
        
        var authorityOptions = new AuthorityAuthenticationOptions();
        configureOptions.Invoke(authorityOptions);

        var authBuilder = builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = authorityOptions.DefaultScheme;
            options.DefaultAuthenticateScheme = authorityOptions.DefaultAuthenticateScheme;
            options.DefaultChallengeScheme = authorityOptions.DefaultChallengeScheme;
            options.DefaultSignInScheme = authorityOptions.DefaultSignInScheme;
            options.DefaultSignOutScheme = authorityOptions.DefaultSignOutScheme;
            options.DefaultForbidScheme = authorityOptions.DefaultForbidScheme;
        });
        return authBuilder;
    }

    public static AuthenticationBuilder AddAuthorityLoginScheme(this AuthenticationBuilder builder, string scheme) =>
        AddAuthorityLoginScheme(builder, scheme, _ => { });
    public static AuthenticationBuilder AddAuthorityLoginScheme(this AuthenticationBuilder builder,
        string scheme,
        Action<AuthorityLoginOptions> configureOptions)
    {
        builder.AddScheme<AuthorityLoginOptions, AuthorityLoginAuthenticationHandler>(scheme, scheme, configureOptions);
        return builder;
    }

    public static AuthenticationBuilder AddAuthorityCookie(this AuthenticationBuilder builder, string scheme)
    {
        builder.AddCookie(scheme, options =>
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

    public static AuthenticationBuilder AddAuthorityToken(this AuthenticationBuilder builder, string scheme)
    {
        
        return builder;
    }

    public static AuthorityBuilder MapAuthorityEndpoints(this AuthorityBuilder builder)
    {
        return builder;
    }
}