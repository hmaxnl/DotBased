using System.Security.Claims;
using DotBased.AspNet.Authority.Models.Options.Auth;
using DotBased.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DotBased.AspNet.Authority.Services;

public class AuthorityAuthenticationService(
    IAuthenticationSchemeProvider schemes,
    IAuthenticationHandlerProvider handlers,
    IClaimsTransformation transform,
    IOptions<AuthenticationOptions> options,
    IOptions<AuthorityAuthenticationOptions> authorityOptions) : AuthenticationService(schemes, handlers, transform, options)
{
    private readonly ILogger _logger = LogService.RegisterLogger(typeof(AuthorityAuthenticationService));
    private readonly AuthorityAuthenticationOptions _options = authorityOptions.Value;
    
    public IReadOnlyCollection<SchemeInfo> GetSchemeInfos(SchemeType schemeType) => _options.SchemeInfoMap.Where(s => s.Type == schemeType).ToList();
    public IReadOnlyCollection<SchemeInfo> GetAllSchemeInfos() => _options.SchemeInfoMap;

    // Validate credentials
    // Used internally by ASP.NET Core to determine if a user is authenticated. Can also be called manually to inspect authentication status.
    public override Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
    {
        return base.AuthenticateAsync(context, scheme);
    }

    // Trigger login
    // Used when access to a resource requires authentication, but the user has not provided valid credentials.
    public override Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return base.ChallengeAsync(context, scheme, properties);
    }

    // Log user in, set cookie/token
    // Called after successfully validating user credentials (e.g., after login form submission), to establish an authenticated session.
    public override Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
    {
        return base.SignInAsync(context, scheme, principal, properties);
    }

    // Log out user and end auth session, remove cookie/token
    public override Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return base.SignOutAsync(context, scheme, properties);
    }

    // Deny access, return 403/return forbid page
    // Used when a user is authenticated but lacks required roles/claims/permissions.
    public override Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return base.ForbidAsync(context, scheme, properties);
    }
}