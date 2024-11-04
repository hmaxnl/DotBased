using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DotBased.ASP.Auth;

public static class BasedAuthDefaults
{
    public const string AuthenticationScheme = "DotBasedAuthentication";
    public const string StorageKey = "dotbased_session";

    public static IComponentRenderMode InteractiveServerWithoutPrerender { get; } =
        new InteractiveServerRenderMode(prerender: false);
}