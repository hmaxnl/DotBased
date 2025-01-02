namespace DotBased.AspNet.Authority.Models.Options;

public class UserOptions
{
    public bool EnableRegister { get; set; }
    public bool RequireUniqueEmail { get; set; }
    public string UserNameCharacters { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
    public ListOption UserNameCharacterListType { get; set; } = ListOption.Whitelist;

    public List<string> UserNameBlackList { get; set; } = ["admin", "administrator", "dev", "developer"];
    public StringComparer UserNameBlackListComparer { get; set; } = StringComparer.OrdinalIgnoreCase;
}