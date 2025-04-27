using DotBased.AspNet.Authority.Models.Data.Auth;

namespace DotBased.AspNet.Authority.Models.Data.System;

public class AboutModel
{
    public string Name { get; set; } = "Authority.Server";
    public List<AuthenticationType> AuthenticationTypes { get; set; } = [];
    public List<AuthenticationSessionType> SessionTypes { get; set; } = [];
}