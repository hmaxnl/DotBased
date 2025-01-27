namespace DotBased.AspNet.Authority.Models.Options;

public class SignInOptions
{
    public bool RequireVerifiedEmail { get; set; }
    public bool RequireVerifiedPhoneNumber { get; set; }
    public bool RequireConfirmedAccount { get; set; }
}