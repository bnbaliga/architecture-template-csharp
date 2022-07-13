using System.Net.Http;

namespace Fairway.Core.Proxy.Http.Model
{
    public class SendRequest : RequestBase
    {
        public object Object { get; set; }       
        public string RouteParameter { get; set; }

        public HttpMethod HttpMethod { get; set; }
    }

    public class PostRequest : RequestBase
    {
        public object Object { get; set; }        
        public string RouteParameter { get; set; }
    }

    public class PutRequest : RequestBase
    {
        public string RouteParameter { get; set; }
        public object Object { get; set; }
    }

    public class PatchRequest : RequestBase
    {
        public string RouteParameter { get; set; }
        public object Object { get; set; }
    }

    public class DeleteRequest : RequestBase
    {
        public string RouteParameter { get; set; }
    }

    public class GetRequest : RequestBase
    {
        public string RouteParameter { get; set; }
    }
}
