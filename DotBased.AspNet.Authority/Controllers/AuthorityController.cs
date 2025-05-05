using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotBased.AspNet.Authority.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorityController : ControllerBase
{
    [HttpGet("auth/login")]
    [AllowAnonymous]
    public async Task<ActionResult> LoginFromSchemeAsync([FromQuery(Name = "s")] string? scheme)
    {
        var cPrincipal = new ClaimsPrincipal();
        await HttpContext.SignInAsync(cPrincipal);
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