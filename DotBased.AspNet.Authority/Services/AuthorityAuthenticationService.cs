using DotBased.AspNet.Authority.Models.Options.Auth;
using DotBased.Logging;
using Microsoft.AspNetCore.Authentication;
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
}