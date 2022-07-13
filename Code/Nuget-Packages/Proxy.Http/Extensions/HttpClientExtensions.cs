using Fairway.Core.Proxy.Http.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Fairway.Core.Proxy.Http.Extensions
{
    public static class HttpClientExtensions 
    {
        /// <summary>
        /// Use Http Post method to send data using HttpClient with CancellationToken
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<(T Response, HttpStatus HttpStatus)> PostAsync<T>(this HttpClient httpClient,
             PostRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));            

            return await HttpProxy.SendAsync<T>(httpClient, new SendRequest
            {
                BaseUrl = request.BaseUrl,
                ContentType = request.ContentType,
                EndPoint = request.EndPoint,
                Headers = request.Headers,
                HttpMethod = HttpMethod.Post,
                Object = request.Object,
                RouteParameter = request.RouteParameter,
                AuthenticationHeader = request.AuthenticationHeader
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Use Http Put method to send data using HttpClient with CancellationToken
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<(T Response, HttpStatus HttpStatus)> PutAsync<T>(this HttpClient httpClient, PutRequest request,
            CancellationToken cancellationToken = default(CancellationToken)) 
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));           

            return await HttpProxy.SendAsync<T>(httpClient, new SendRequest
            {
                BaseUrl = request.BaseUrl,
                ContentType = request.ContentType,
                EndPoint = request.EndPoint,
                Headers = request.Headers,
                HttpMethod = HttpMethod.Put,
                RouteParameter = request.RouteParameter,
                Object = request.Object,
                AuthenticationHeader = request.AuthenticationHeader
            }, cancellationToken).ConfigureAwait(false);
        }        

        /// <summary>
        /// Use Http Patch method to send data using HttpClient with CancellationToken
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<(T Response, HttpStatus HttpStatus)> PatchAsync<T>(this HttpClient httpClient, PatchRequest request, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));            

            return await HttpProxy.SendAsync<T>(httpClient, new SendRequest
            {
                BaseUrl = request.BaseUrl,
                ContentType = request.ContentType,
                EndPoint = request.EndPoint,
                Headers = request.Headers,
                HttpMethod = new HttpMethod("PATCH"),
                Object = request.Object,
                RouteParameter = request.RouteParameter,
                AuthenticationHeader = request.AuthenticationHeader
            }, cancellationToken).ConfigureAwait(false);
        }        

        /// <summary>
        /// Use Http Delete method to delete data using HttpClient with CancellationToken
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<(T Response, HttpStatus HttpStatus)> DeleteAsync<T>(this HttpClient httpClient, DeleteRequest request, 
            CancellationToken cancellationToken = default(CancellationToken)) 
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));           

            return await HttpProxy.SendAsync<T>(httpClient, new SendRequest
            {
                BaseUrl = request.BaseUrl,
                ContentType = request.ContentType,
                EndPoint = request.EndPoint,
                Headers = request.Headers,
                HttpMethod = HttpMethod.Delete,
                RouteParameter = request.RouteParameter,
                AuthenticationHeader = request.AuthenticationHeader
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Use Http Get method to get json data using HttpClient
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="httpCompletionOption"> This will enable better performance for reading heavy json files
        /// when HttpCompletionOption is send as ResponseHeadersRead
        /// </param>
        /// <returns></returns>
        public static async Task<(T Response, HttpStatus HttpStatus)> GetJsonAsync<T>(this HttpClient httpClient, GetRequest request,
            CancellationToken cancellationToken = default(CancellationToken), HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));            

            return await HttpProxy.SendAsync<T>(httpClient, new SendRequest
            {
                BaseUrl = request.BaseUrl,
                ContentType = request.ContentType,
                EndPoint = request.EndPoint,
                Headers = request.Headers,
                HttpMethod = HttpMethod.Get,
                RouteParameter = request.RouteParameter,
                AuthenticationHeader = request.AuthenticationHeader
            }, cancellationToken, httpCompletionOption).ConfigureAwait(false);
        }        

        /// <summary>
        /// Use Http Get method to get stream data or heavy json file using HttpClient with CancellationToken
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<(T Response, HttpStatus HttpStatus)> GetStreamAsync<T>(this HttpClient httpClient, GetRequest request, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await HttpProxy.SendAsync<T>(httpClient, new SendRequest
            {
                BaseUrl = request.BaseUrl,
                ContentType = request.ContentType,
                EndPoint = request.EndPoint,
                Headers = request.Headers,
                HttpMethod = HttpMethod.Get,
                RouteParameter = request.RouteParameter,
                AuthenticationHeader = request.AuthenticationHeader
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Use Http Get method to get stream data or heavy json file using HttpClient with CancellationToken
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<(T Response, HttpStatus HttpStatus)> SendAsync<T>(this HttpClient httpClient, SendRequest request,
            CancellationToken cancellationToken = default(CancellationToken), HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
      
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

                return await HttpProxy.SendAsync<T>(httpClient, request, cancellationToken, httpCompletionOption).ConfigureAwait(false);
        }
    }
}
