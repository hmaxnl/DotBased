using System.Security.Claims;
using DotBased.AspNet.Authority.Models.Options.Auth;
using DotBased.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DotBased.AspNet.Authority.Services;

public class AuthorityAuthenticationService(
    IAuthenticationHandlerProvider handlers,
    IClaimsTransformation transform,
    IOptions<AuthorityAuthenticationOptions> options) : IAuthenticationService
{
    private readonly ILogger _logger = LogService.RegisterLogger(typeof(AuthorityAuthenticationService));
    private readonly AuthorityAuthenticationOptions _options = options.Value;
    
    public async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
    {
        _logger.Debug("Authenticate with scheme: {Scheme}", scheme);
        
        var authenticationHandler = await GetAuthenticationHandler(context, scheme, _options.DefaultAuthenticateScheme);
        var authResult = await authenticationHandler.AuthenticateAsync();

        return authResult is { Succeeded: true }
            ? AuthenticateResult.Success(
                new AuthenticationTicket(await transform.TransformAsync(authResult.Principal), authResult.Properties, authResult.Ticket.AuthenticationScheme)) :
            AuthenticateResult.Fail("Failed to authenticate");
    }

    public async Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        _logger.Debug("Challenging with scheme: {Scheme}", scheme);
        
        var authenticationHandler = await GetAuthenticationHandler(context, scheme, _options.DefaultChallengeScheme);
        await authenticationHandler.ChallengeAsync(properties);
    }

    public async Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        _logger.Debug("Forbid with scheme: {Scheme}", scheme);
        
        var authenticationHandler = await GetAuthenticationHandler(context, scheme, _options.DefaultForbidScheme);
        await authenticationHandler.ForbidAsync(properties);
    }

    public async Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
    {
        _logger.Debug("SignIn with scheme: {Scheme}", scheme);
        
        var authenticationHandler = await GetAuthenticationHandler(context, scheme, _options.DefaultSignInScheme);
        if (authenticationHandler is not IAuthenticationSignInHandler signInHandler)
        {
            throw new InvalidOperationException("Authentication handler is not a IAuthenticationSignInHandler.");
        }
        await signInHandler.SignInAsync(principal, properties);
    }

    public async Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        _logger.Debug("SignOut with scheme: {Scheme}", scheme);
        
        var authenticationHandler = await GetAuthenticationHandler(context, scheme, _options.DefaultSignOutScheme);
        if (authenticationHandler is not IAuthenticationSignOutHandler signOutHandler)
        {
            throw new InvalidOperationException("Authentication handler is not a IAuthenticationSignOutHandler.");
        }
        await signOutHandler.SignOutAsync(properties);
    }

    /*public async Task ValidateLoginAsync()
    {
        //TODO: Check if user is logged in from external identity provider, if user not exists in authority db create user.
        throw new NotImplementedException();
    }*/

    private async Task<IAuthenticationHandler> GetAuthenticationHandler(HttpContext context, string scheme, string defaultScheme)
    {
        if (string.IsNullOrWhiteSpace(scheme))
        {
            scheme = defaultScheme;
            if (string.IsNullOrWhiteSpace(scheme))
            {
                scheme = _options.DefaultScheme;
                if (string.IsNullOrWhiteSpace(scheme))
                {
                    throw new InvalidOperationException("Failed to get default scheme. No scheme specified.");
                }
            }
        }
        
        var authenticationHandler = await handlers.GetHandlerAsync(context, scheme);
        if (authenticationHandler == null)
        {
            _logger.Warning("Failed to load handler for scheme: {Scheme}", scheme);
            throw new InvalidOperationException($"No authentication handlers registered to the scheme '{scheme}'");
        }
        
        return authenticationHandler;
    }
}