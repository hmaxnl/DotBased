namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityUserItem
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? EmailAddress { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
}