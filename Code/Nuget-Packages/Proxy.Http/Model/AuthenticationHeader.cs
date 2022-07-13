namespace Fairway.Core.Proxy.Http.Model
{
    public class AuthenticationHeader
    {
        //
        // Summary:
        //     Gets the credentials containing the authentication information of the user agent
        //     for the resource being requested.
        //
        // Returns:
        //     The credentials containing the authentication information.
        public string Parameter { get; set; }
        //
        // Summary:
        //     Gets the scheme to use for authorization.
        //
        // Returns:
        //     The scheme to use for authorization.
        public string Scheme { get; set; }
    }
}
