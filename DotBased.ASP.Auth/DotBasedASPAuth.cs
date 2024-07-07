using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DotBased.ASP.Auth;

public static class DotBasedASPAuth
{
    public static void UseBasedAuth(this WebApplicationBuilder builder, BasedAuthConfiguration configuration)
    {
        builder.Services.AddScoped<AuthenticationStateProvider, BasedAuthenticationStateProvider>();
    }
}