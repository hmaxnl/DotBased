using System.Text;
using DotBased.AspNet.Authority.Attributes;

namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityUser()
{
    public AuthorityUser(string userName) : this()
    {
        UserName = userName;
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public bool Enabled { get; set; }
    
    public bool Confirmed { get; set; }
    
    public bool Locked { get; set; }

    public DateTime LockedDate { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? PasswordHash { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public bool TwoFactorEnabled { get; set; }

    public long Version { get; set; }

    public long SecurityVersion { get; set; }
    
    [Protect]
    public string? EmailAddress { get; set; }

    public bool EmailConfirmed { get; set; }

    [Protect]
    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public ICollection<AuthorityAttribute> Attributes { get; set; } = [];

    public override string ToString()
    {
        var strBuilder = new StringBuilder();
        strBuilder.Append(!string.IsNullOrWhiteSpace(Name) ? Name : UserName);

        if (string.IsNullOrWhiteSpace(EmailAddress)) return strBuilder.ToString();
        
        strBuilder.Append(strBuilder.Length == 0 ? EmailAddress : $" ({EmailAddress})");

        return strBuilder.ToString();
    }
}