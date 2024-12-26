using DotBased.AspNet.Authority.Attributes;

namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityUser : AuthorityUser<Guid>
{
    public AuthorityUser(string userName) : this()
    {
        UserName = userName;
    }
    
    public AuthorityUser()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.Now;
    }
}

public abstract class AuthorityUser<TKey> : AuthorityUserBase where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
}

public abstract class AuthorityUserBase
{
    public bool Enabled { get; set; }
    
    public bool Confirmed { get; set; }
    
    public bool Locked { get; set; }

    public DateTime LockedDate { get; set; }

    public string? UserName { get; set; }

    public string? PasswordHash { get; set; }
    
    public DateTime CreatedDate { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public long Version { get; set; }

    public long SecurityVersion { get; set; }
    
    [Protect]
    public string? EmailAddress { get; set; }

    public bool EmailConfirmed { get; set; }

    [Protect]
    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public override string ToString() => UserName ?? EmailAddress ?? string.Empty;
}