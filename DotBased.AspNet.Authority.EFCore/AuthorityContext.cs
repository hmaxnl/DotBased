using DotBased.AspNet.Authority.Models.Authority;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore;

public class AuthorityContext(DbContextOptions<AuthorityContext> options) : DbContext(options)
{
    public DbSet<AuthorityAttribute> Attributes { get; set; }
    public DbSet<AuthorityGroup> Groups { get; set; }
    public DbSet<AuthorityRole> Roles { get; set; }
    public DbSet<AuthorityUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorityAttribute>(attributeEntity =>
        {
            attributeEntity.ToTable("authority_attributes");
            attributeEntity.HasKey(a => new { a.BoundId, a.AttributeKey });
        });

        modelBuilder.Entity<AuthorityGroup>(groupEntity =>
        {
            groupEntity.ToTable("authority_groups");
            groupEntity.HasKey(x => x.Id);
            groupEntity.HasMany(g => g.Attributes).WithOne().HasForeignKey(a => a.BoundId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AuthorityRole>(roleEntity =>
        {
            roleEntity.ToTable("authority_roles");
            roleEntity.HasKey(x => x.Id);
        });
        
        modelBuilder.Entity<AuthorityUser>(userEntity =>
        {
            userEntity.ToTable("authority_users");
            userEntity.HasKey(x => x.Id);
            userEntity.HasMany(u => u.Attributes).WithOne().HasForeignKey(a => a.BoundId).OnDelete(DeleteBehavior.Cascade);
        });
        
        base.OnModelCreating(modelBuilder);
    }
}