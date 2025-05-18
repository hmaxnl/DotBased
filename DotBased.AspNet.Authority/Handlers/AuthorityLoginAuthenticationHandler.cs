using System.Buffers.Text;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Options.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace DotBased.AspNet.Authority.Handlers;

/// <summary>
/// Handles authentication for Authority logins.
/// </summary>
public class AuthorityLoginAuthenticationHandler(IOptionsMonitor<AuthorityLoginOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    AuthorityManager manager) : AuthenticationHandler<AuthorityLoginOptions>(options, logger, encoder)
{
    // Validate credentials
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authResult = GetBasicAuthorization(out var email, out var password);
        if (authResult != null || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return AuthenticateResult.Fail(authResult ?? "Failed to get basic authorization from header.");
        }
        
        var userResult = await manager.GetUserByEmailAsync(email);
        if (userResult is { IsSuccess: false, Error: not null })
        {
            return AuthenticateResult.Fail(userResult.Error.Description);
        }
        var user = userResult.Value;
        
        var passwordValidateResult = await manager.ValidatePasswordAsync(user, password);
        if (!passwordValidateResult.IsSuccess)
        {
            return AuthenticateResult.Fail(passwordValidateResult.Error?.Description ?? "Failed to validate password.");
        }

        var identityClaims = new List<Claim>();
        var rolesResult = await manager.GetAllUserRolesAsync(user);
        if (rolesResult.IsSuccess)
        {
            var roles = rolesResult.Value;
            foreach (var authorityRole in roles)
            {
                identityClaims.Add(new Claim(ClaimTypes.Role, authorityRole.Name));
            }
        }
        
        var principal = new ClaimsPrincipal(new ClaimsIdentity(identityClaims, Scheme.Name));
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        var result = AuthenticateResult.Success(ticket);
        return result;
    }

    private string? GetBasicAuthorization(out string? email, out string? password)
    {
        email = null;
        password = null;
        
        if (StringValues.IsNullOrEmpty(Context.Request.Headers.Authorization))
        {
            return "Missing authorization header";
        }

        var basicAuth = string.Empty;
        foreach (var authorizationValue in Context.Request.Headers.Authorization)
        {
            if (string.IsNullOrWhiteSpace(authorizationValue))
            {
                continue;
            }

            if (AuthenticationHeaderValue.TryParse(authorizationValue, out var basicAuthHeader) && !string.IsNullOrWhiteSpace(basicAuthHeader.Parameter))
            {
                basicAuth = basicAuthHeader.Parameter;
            }
        }

        if (!Base64.IsValid(basicAuth))
        {
            return "Invalid basic authorization data!";
        }
        
        var base64Auth = Convert.FromBase64String(basicAuth);
        var decodedAuth = System.Text.Encoding.UTF8.GetString(base64Auth);
        var parts = decodedAuth.Split(':');
        if (parts.Length != 2)
        {
            return "No email and/or password found!";
        }

        email = parts[0];
        password = parts[1];
        return null;
    }
}