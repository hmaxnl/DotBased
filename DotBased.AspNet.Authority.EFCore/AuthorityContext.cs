using DotBased.AspNet.Authority.Models.Authority;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore;

public class AuthorityContext : DbContext
{
    public DbSet<AuthorityAttribute> Attributes { get; set; }
    public DbSet<AuthorityGroup> Groups { get; set; }
    public DbSet<AuthorityRole> Roles { get; set; }
    public DbSet<AuthorityUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorityAttribute>().ToTable("authority_attributes");
        modelBuilder.Entity<AuthorityGroup>().ToTable("authority_groups");
        modelBuilder.Entity<AuthorityRole>().ToTable("authority_roles");
        modelBuilder.Entity<AuthorityUser>().ToTable("authority_users");
        base.OnModelCreating(modelBuilder);
    }
}