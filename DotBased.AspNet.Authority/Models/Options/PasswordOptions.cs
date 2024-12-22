namespace DotBased.AspNet.Authority.Models.Options;

public class PasswordOptions
{
    public int RequiredLength { get; set; } = 10;
    public int MinimalUniqueChars { get; set; } = 1;
    public bool RequireLowercase { get; set; }
    public bool RequireUppercase { get; set; }
    public bool RequireDigit { get; set; }
    public bool RequireNonAlphanumeric { get; set; }

    public List<string> PasswordBlackList { get; set; } = ["password", "1234"];
    public StringComparer PasswordBlackListComparer { get; set; } = StringComparer.OrdinalIgnoreCase;
}