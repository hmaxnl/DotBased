using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotBased.AspNet.Authority.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorityController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpGet("auth/login")]
    [AllowAnonymous]
    public async Task<ActionResult> LoginFromSchemeAsync([FromQuery(Name = "s")] string? scheme, [FromQuery(Name = "ss")] string? sessionScheme)
    {
        await authenticationService.AuthenticateAsync(HttpContext, scheme);
        return Ok();
    }

    [HttpGet("auth/logout")]
    public async Task<ActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }

    [HttpGet("info")]
    [AllowAnonymous]
    public async Task<ActionResult<JsonDocument>> GetAuthorityInfoAsync()
    {
        return Ok();
    }
}