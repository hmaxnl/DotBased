namespace DotBased.AspNet.Authority;

public static class AuthorityDefaults
{
    public static class Scheme
    {
        public static class Authority
        {
            public const string AuthenticationScheme = "AuthorityLogin";
        }
        
        public static class Cookie
        {
            public const string AuthenticationScheme = "AuthorityCookie";
            public const string CookieName = "AuthorityAuth";
        }
        
        public static class Token
        {
            public const string AuthenticationScheme = "AuthorityToken";
            public const string TokenName = "AuthorityAuthToken";
        }
    }
    
    public static class Paths
    {
        public const string Default = "/";
        public const string Login = "/auth/login";
        public const string Logout = "/auth/logout";
        public const string Challenge = "/auth/challenge";
        public const string Forbidden = "/forbidden";
        public const string Info = "/info";
    }
    
    public static class ClaimTypes
    {
        public const string Attribute = "Authority.Attribute";
        public const string AuthenticatedScheme = "Authority.Scheme.Authenticated";
    }
}