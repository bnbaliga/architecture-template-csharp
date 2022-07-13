using Fairway.Core.Proxy.Http.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fairway.Core.Proxy.Http
{
    internal static class HttpProxy
    {

        public const string BaseAddressCannotBeEmpty = "Base Address is null or empty";
        public const string InValidContentType = "Invalid content type";

        internal static class ContentType
        {
            public const string Json = "application/json";
            public const string UrlEncoded = "application/x-www-form-urlencoded";
            public const string ApplicationXML = "application/xml";
            public const string TextXML = "text/xml";
        }

        internal static class HttpHeaders
        {
            public const string Connection = "Connection";
            public const string KeepAlive = "Keep-Alive";
        }

        internal const string Schema = "Bearer";
        internal const string FunctionAuthorizationHeader = "Authorization";

        internal static async Task<(T Response, HttpStatus HttpStatus)> SendAsync<T>(HttpClient httpClient,
            SendRequest request, CancellationToken cancellationToken = default(CancellationToken),
                    HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            using (var httpRequestMessage = new HttpRequestMessage())
            {
                Uri requestUri = new Uri(FormatBaseAddress(request.BaseUrl) + Path.Combine(request.EndPoint, request.RouteParameter ?? string.Empty));

                var property = typeof(T).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).
                            Where(x => x.PropertyType == typeof(Stream)).FirstOrDefault();

                // If Response type T is Stream or if T is a complex type and one  of its property is Stream
                bool isResponseContainStream = (typeof(T) == typeof(Stream)) || property != null;
                if (isResponseContainStream)
                    httpCompletionOption = HttpCompletionOption.ResponseHeadersRead;

                HttpResponseMessage result = await SendHttpRequestMessageAsync(httpClient, request.HttpMethod,
                 request.Object, requestUri, request, httpRequestMessage, cancellationToken, httpCompletionOption).ConfigureAwait(false);

                if (result.IsSuccessStatusCode)
                {
                    if (!isResponseContainStream)
                    {
                        if (httpCompletionOption == HttpCompletionOption.ResponseHeadersRead)
                        {
                            using (var responseStream = await result.Content.ReadAsStreamAsync().ConfigureAwait(false))
                            {
                                using (var streamReader = new StreamReader(responseStream))
                                using (var jsonTextReader = new JsonTextReader(streamReader))
                                {
                                    var jsonSerializer = new JsonSerializer();
                                    var jsonResult = jsonSerializer.Deserialize<T>(jsonTextReader);

                                    return (jsonResult, new HttpStatus() { IsSucessStatusCode = result.IsSuccessStatusCode });
                                }
                            }
                        }
                        else
                        {
                            var response = await ReadResponseAsStringAsync<T>(result, request.ContentType).ConfigureAwait(false);

                            return (response.Response, response.HttpStatus);
                        }
                    }
                    else
                    {
                        var streamResponse = await result.Content.ReadAsStreamAsync().ConfigureAwait(false);
                        if (property != null)
                        {
                            var streamResponseObject = Activator.CreateInstance<T>();
                            property.SetValue(streamResponseObject, streamResponse);

                            return (streamResponseObject, new HttpStatus() { IsSucessStatusCode = result.IsSuccessStatusCode });
                        }
                        else
                            throw new InvalidOperationException($"{nameof(T)} doesn't contain property type as stream");
                    }
                }
                else
                {
                    var jsonResponse = await ReadResponseAsStringAsync<T>(result, request.ContentType).ConfigureAwait(false);

                    return (jsonResponse.Response, jsonResponse.HttpStatus);
                }
            }
        }

        private static async Task<HttpResponseMessage> SendHttpRequestMessageAsync(HttpClient httpClient, HttpMethod httpMethod,
            object model, Uri requestUri,
            RequestBase request, HttpRequestMessage httpRequestMessage,
            CancellationToken cancellationToken,
            HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
        {
            if (model != null)
                httpRequestMessage.Content = SetContent(model, request.ContentType);

            httpRequestMessage.Method = httpMethod;
            httpRequestMessage.RequestUri = requestUri;
            SetAuthHeader(request.AuthenticationHeader, httpRequestMessage);
            SetHeaders(request.Headers, httpRequestMessage);        

            var result = await httpClient.SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken).ConfigureAwait(false);

            return result;          
        }       

        private static async Task<(T Response, HttpStatus HttpStatus)> ReadResponseAsStringAsync<T>(HttpResponseMessage result, string contentType)
        {
            if (result == null)
            {
                return (default, default);
            }

            var responseHeaders = GetResponseHeaders(result.Headers);

            var response = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
            {
                if (string.Equals(contentType, ContentType.ApplicationXML, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(contentType, ContentType.TextXML, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (typeof(T) != typeof(string))
                        throw new InvalidOperationException($"For requests with xml content type, return type should be string. Requested type '{typeof(T).Name}' is not supported.");

                    return ((T)Convert.ChangeType(response, typeof(T)), new HttpStatus()
                    {
                        IsSucessStatusCode = result.IsSuccessStatusCode,
                        ResponeHeaders = responseHeaders
                    });
                }
                
                if (response.StartsWith("["))
                {
                    var resultObject = Activator.CreateInstance<T>();
                    var property = resultObject.GetType().GetProperties().FirstOrDefault();
                    if (property != null)
                    {
                        property.SetValue(resultObject, JsonConvert.DeserializeObject(response, property.PropertyType));

                        return (resultObject, new HttpStatus() { IsSucessStatusCode = result.IsSuccessStatusCode, ResponeHeaders = responseHeaders });

                    }
                }
                var jsonResult = JsonConvert.DeserializeObject<T>(response);

                return (jsonResult, new HttpStatus() { IsSucessStatusCode = result.IsSuccessStatusCode, ResponeHeaders = responseHeaders });

            }

            return (default, new HttpStatus
            {
                StatusCode = result.StatusCode,
                ErrorReason = result.ReasonPhrase,
                ErrorDescription = response,
                IsSucessStatusCode = result.IsSuccessStatusCode,
                ResponeHeaders = responseHeaders
            });
        }
        private static void SetHeaders(Dictionary<string, string> requestHeaders, HttpRequestMessage request)
        {
            if (requestHeaders != null)
            {
                foreach (var header in requestHeaders)
                    request.Headers.Add(header.Key, header.Value);
            }
        }

        private static void SetAuthHeader(AuthenticationHeader authenticationHeader, HttpRequestMessage request)
        {
            if (authenticationHeader != null)
                request.Headers.Authorization = new AuthenticationHeaderValue(authenticationHeader.Scheme,
                                                            authenticationHeader.Parameter);
        }

        private static string FormatBaseAddress(string baseAddress)
        {
            if (string.IsNullOrEmpty(baseAddress))
                throw new ArgumentException(BaseAddressCannotBeEmpty);

            return !baseAddress.EndsWith("/") ? baseAddress + "/" : baseAddress;
        }

        private static HttpContent SetContent(object model, string contentType)
        {
            if (model == null)
                return null;

            HttpContent httpContent;
            switch (contentType.ToLower())
            {
                case ContentType.UrlEncoded:
                    httpContent = model as FormUrlEncodedContent;
                    break;

                case ContentType.Json:
                    httpContent = GetJsonContent(model);
                    break;

                case ContentType.ApplicationXML:
                case ContentType.TextXML:
                    httpContent = new StringContent(model.ToString(), Encoding.UTF8, contentType);
                    break;

                default:
                    throw new ArgumentException(InValidContentType);
            }

            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue(contentType);

            return httpContent;
        }

        private static HttpContent GetJsonContent(object model)
        {
            HttpContent httpContent;
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            httpContent = new StringContent(JsonConvert.SerializeObject(model, Formatting.None, serializerSettings));
            return httpContent;
        }

        private static CancellationTokenSource GetCancellationTokenSource(HttpClient httpClient, CancellationToken cancellationToken)
        {
            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationTokenSource.CancelAfter(httpClient.Timeout);
            return cancellationTokenSource;
        }

        private static Dictionary<string, IEnumerable<string>> GetResponseHeaders(HttpResponseHeaders httpResponseHeaders)
        {
            var responseHeaders = new Dictionary<string, IEnumerable<string>>();
            foreach (var item in httpResponseHeaders)
            {
                responseHeaders.Add(item.Key, item.Value);
            }

            return responseHeaders;
        }
    }
}
