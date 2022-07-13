using System.Collections.Generic;

namespace Fairway.Core.Proxy.Http.Model
{
    public class RequestBase
    {
        public string BaseUrl { get; set; }
        public string EndPoint { get; set; }
        public string ContentType { get; set; }
        public AuthenticationHeader AuthenticationHeader { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}
