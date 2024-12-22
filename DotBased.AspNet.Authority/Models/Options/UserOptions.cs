namespace DotBased.AspNet.Authority.Models.Options;

public class UserOptions
{
    public bool EnableRegister { get; set; }
    public bool RequireUniqueEmail { get; set; }
    public string AllowedCharacters { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";

    public List<string> UserNameBlackList { get; set; } = ["admin", "administrator", "dev", "developer"];
    public StringComparer UserNameBlackListComparer { get; set; } = StringComparer.OrdinalIgnoreCase;
}