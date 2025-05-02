using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace DotBased.AspNet.Authority.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class AuthorityController : ControllerBase
{
    [Inject]
    public IAuthenticationService AuthenticationService { get; set; }
    
    [HttpGet("auth/login")]
    [AllowAnonymous]
    public async Task<ActionResult> LoginFromSchemeAsync([FromQuery(Name = "s")] string? scheme)
    {
        var authResult = await HttpContext.AuthenticateAsync();
        return Ok();
    }

    [HttpGet("auth/logout")]
    public async Task<ActionResult> LogoutAsync()
    {
        return Ok();
    }

    [HttpGet("info")]
    public async Task<ActionResult<JsonDocument>> GetAuthorityInfoAsync()
    {
        return Ok();
    }
}