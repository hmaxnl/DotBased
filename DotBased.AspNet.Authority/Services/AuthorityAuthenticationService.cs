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

    public override Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
    {
        
        return base.AuthenticateAsync(context, scheme);
    }

    public override Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return base.ChallengeAsync(context, scheme, properties);
    }

    public override Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
    {
        return base.SignInAsync(context, scheme, principal, properties);
    }

    public override Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return base.SignOutAsync(context, scheme, properties);
    }

    public override Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return base.ForbidAsync(context, scheme, properties);
    }
}