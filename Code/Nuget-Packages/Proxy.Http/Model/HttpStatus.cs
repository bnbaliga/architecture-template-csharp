using System.Collections.Generic;
using System.Net;

namespace Fairway.Core.Proxy.Http.Model
{
    public class HttpStatus
    {
        public bool IsSucessStatusCode { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string ErrorReason { get; set; }
        public string ErrorDescription { get; set; }
        public Dictionary<string, IEnumerable<string>> ResponeHeaders { get; set; }
    }
}
