namespace DotBased.AspNet.Authority.Models.Authority;

public class AuthorityUser : AuthorityUserBase<Guid>
{
    public AuthorityUser()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.Now;
    }
}