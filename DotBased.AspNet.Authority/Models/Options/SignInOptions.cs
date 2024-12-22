namespace DotBased.AspNet.Authority.Models.Options;

public class SignInOptions
{
    public bool RequireValidatedEmail { get; set; }
    public bool RequireValidatedPhoneNumber { get; set; }
    public bool RequireConfirmedAccount { get; set; }
}