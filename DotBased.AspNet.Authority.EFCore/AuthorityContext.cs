using DotBased.AspNet.Authority.EFCore.Models;
using DotBased.AspNet.Authority.Models.Authority;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore;

public class AuthorityContext(DbContextOptions<AuthorityContext> options) : DbContext(options)
{
    public DbSet<AuthorityAttribute> Attributes { get; set; }
    public DbSet<AuthorityGroup> Groups { get; set; }
    public DbSet<AuthorityRole> Roles { get; set; }
    public DbSet<AuthorityUser> Users { get; set; }
    
    public DbSet<RoleGroup> RoleGroup { get; set; }
    public DbSet<RoleUser> RoleUser { get; set; }
    public DbSet<UserGroup> UserGroup { get; set; }

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
            roleEntity.HasMany(r => r.Attributes).WithOne().HasForeignKey(a => a.BoundId).OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<AuthorityUser>(userEntity =>
        {
            userEntity.ToTable("authority_users");
            userEntity.HasKey(x => x.Id);
            userEntity.HasMany(u => u.Attributes).WithOne().HasForeignKey(a => a.BoundId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RoleGroup>(rgEntity =>
        {
            rgEntity.ToTable("role_group");
            rgEntity.HasKey(rg => new { rg.RoleId, rg.GroupId });
        });

        modelBuilder.Entity<RoleUser>(ruEntity =>
        {
            ruEntity.ToTable("role_user");
            ruEntity.HasKey(ru => new { ru.RoleId, ru.UserId });
        });

        modelBuilder.Entity<UserGroup>(ugEntity =>
        {
            ugEntity.ToTable("user_group");
            ugEntity.HasKey(ug => new { ug.UserId, ug.GroupId });
        });
        
        base.OnModelCreating(modelBuilder);
    }
}