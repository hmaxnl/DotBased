namespace DotBased.AspNet.Authority;

public static class AuthorityDefaults
{
    public static class Scheme
    {
        public static class Cookie
        {
            public const string Default = "Authority.Scheme.Cookie";
            public const string CookieName = "AuthorityAuth";
        }
        
        public static class Token
        {
            public const string Default = "Authority.Scheme.Token";
            public const string TokenName = "AuthorityAuthToken";
        }
    }
    
    public static class Paths
    {
        public const string Default = "/";
        public const string Login = "/auth/login";
        public const string Logout = "/auth/logout";
        public const string Forbidden = "/forbidden";
    }
}