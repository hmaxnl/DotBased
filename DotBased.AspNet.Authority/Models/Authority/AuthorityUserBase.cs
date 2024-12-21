using DotBased.AspNet.Authority.Attributes;

namespace DotBased.AspNet.Authority.Models.Authority;

public abstract class AuthorityUserBase<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
    
    public bool Enabled { get; set; }
    
    public bool Locked { get; set; }

    public string UserName { get; set; }

    public string PasswordHash { get; set; }
    
    public DateTime CreatedDate { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public string ConcurrencyStamp { get; set; }
    public string SecurityStamp { get; set; }
    
    
    [Protect]
    public string EmailAddress { get; set; }

    public bool EmailConfirmed { get; set; }

    [Protect]
    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }
    
}