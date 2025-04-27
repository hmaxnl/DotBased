using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DotBased.AspNet.Authority.Services;

public class AuthorityAuthenticationService(IAuthenticationSchemeProvider schemes, IAuthenticationHandlerProvider handlers, IClaimsTransformation transform) : AuthenticationService(schemes, handlers, transform)
{
    public override Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
    {
        //TODO: Get from query parameters which auth scheme to use or fallback to configured default.
        return base.SignInAsync(context, scheme, principal, properties);
    }

    public override Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        //TODO: Figure out which type of auth is used and logout with the scheme.
        return base.SignOutAsync(context, scheme, properties);
    }
}