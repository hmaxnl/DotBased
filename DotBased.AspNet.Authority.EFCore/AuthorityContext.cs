using DotBased.AspNet.Authority.Models.Authority;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore;

public class AuthorityContext : DbContext
{
    public DbSet<AuthorityAttribute> Attributes { get; set; }
    public DbSet<AuthorityGroup> Groups { get; set; }
    public DbSet<AuthorityRole> Roles { get; set; }
    public DbSet<AuthorityUser> Users { get; set; }
}