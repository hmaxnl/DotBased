namespace DotBased.AspNet.Authority.EFCore.Models;

public class RoleUser
{
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }
}