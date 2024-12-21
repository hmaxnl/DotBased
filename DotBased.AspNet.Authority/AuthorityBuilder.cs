using Microsoft.Extensions.DependencyInjection;

namespace DotBased.AspNet.Authority;

public class AuthorityBuilder
{
    public AuthorityBuilder(IServiceCollection services)
    {
        Services = services;
    }
    
    public IServiceCollection Services { get; }
}