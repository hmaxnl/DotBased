using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace DotBased.AspNet.Authority.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorityController : ControllerBase
{
    [HttpGet("auth/login")]
    public async Task<ActionResult> LoginFromSchemeAsync([FromQuery(Name = "s")] string scheme)
    {
        return BadRequest();
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