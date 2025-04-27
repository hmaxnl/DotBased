using System.Security.Claims;
using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Options.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DotBased.AspNet.Authority.Services;

public class AuthorityAuthenticationService(IAuthenticationSchemeProvider schemes,
    IAuthenticationHandlerProvider handlers,
    IClaimsTransformation transform,
    IOptions<AuthorityAuthenticationOptions> options,
    AuthorityManager manager) : IAuthenticationService
{
    public async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
    {
        throw new NotImplementedException();
    }

    public async Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }

    public async Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }

    public async Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }

    public async Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }

    public async Task ValidateLoginAsync()
    {
        
        //TODO: Check if user is logged in from external identity provider, if user not exists in authority db create user.
        throw new NotImplementedException();
    }
}