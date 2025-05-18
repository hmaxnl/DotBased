using System.Text.Json;
using DotBased.AspNet.Authority.Models.Data.System;
using DotBased.AspNet.Authority.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotBased.AspNet.Authority.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorityController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpGet(AuthorityDefaults.Paths.Login)]
    [AllowAnonymous]
    public async Task<ActionResult> LoginFromSchemeAsync([FromQuery(Name = "s")] string? scheme, [FromQuery(Name = "ss")] string? sessionScheme)
    {
        await authenticationService.AuthenticateAsync(HttpContext, scheme);
        return Ok();
    }

    [HttpGet(AuthorityDefaults.Paths.Challenge)]
    [AllowAnonymous]
    public IActionResult ChallengeLogin([FromQuery(Name = "s")] string? scheme, [FromQuery(Name = "returnUrl")] string returnUrl = "/")
    {
        return Challenge(scheme, returnUrl);
    }

    [HttpGet(AuthorityDefaults.Paths.Logout)]
    public async Task<ActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }

    [HttpGet(AuthorityDefaults.Paths.Info)]
    [AllowAnonymous]
    public async Task<ActionResult<JsonDocument>> GetAuthorityInfoAsync()
    {
        if (authenticationService is not AuthorityAuthenticationService authService)
        {
            return BadRequest();
        }

        var schemesInfos = authService.GetAllSchemeInfos();
        
        var info = new AuthorityInformation
        {
            IsAuthenticated = false,
            SchemeInformation = new SchemeInformation
            {
                DefaultScheme = authService.Options.DefaultScheme ?? "Unknown",
                AvailableSchemes = schemesInfos.ToList()
            }
        };
        
        return Ok(info);
    }
}