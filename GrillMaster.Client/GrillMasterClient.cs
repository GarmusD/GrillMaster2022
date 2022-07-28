using GrillMaster.Data.DTO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace GrillMaster.Client
{
    public class GrillMasterClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        public GrillMasterClient(Uri baseUri)
        {
            _httpClient = new()
            {
                BaseAddress = baseUri
            };
        }

        /// <summary>
        /// Authenticate to API server
        /// </summary>
        /// <param name="authUserRequest">Username and Password</param>
        /// <returns></returns>
        /// <exception cref="GrillMasterOfflineException"/>
        /// <exception cref="GrillMasterNotAuthenticatedException"/>
        public Task<string> AuthenticateAsync(AuthUserRequest authUserRequest)
        {
            return AuthenticateAsync(authUserRequest, CancellationToken.None);
        }

        /// <summary>
        /// Authenticate to API server
        /// </summary>
        /// <param name="authUserRequest">Username and Password</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        /// <exception cref="GrillMasterOfflineException"></exception>
        /// <exception cref="GrillMasterNotAuthenticatedException"></exception>
        public async Task<string> AuthenticateAsync(AuthUserRequest authUserRequest, CancellationToken token)
        {            
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/user/login");
            request.Headers.Add("Accept", "text/plain");
            request.Content = JsonContent.Create(authUserRequest);

            var response = await _httpClient.SendAsync(request, token);

            if (response.IsSuccessStatusCode)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await response.Content.ReadAsStringAsync(token));
                return await response.Content.ReadAsStringAsync(token);
            }
            else if (response.StatusCode == 0)
                throw new GrillMasterOfflineException();

            throw new GrillMasterNotAuthenticatedException();
        }

        /// <summary>
        /// Optimize grill order.
        /// </summary>
        /// <param name="grillOrder"></param>
        /// <returns>Returns optimized grill order.</returns>
        /// <exception cref="GrillMasterInvalidJsonException"/>
        /// <exception cref="GrillMasterApiErrorException"/>
        /// <exception cref="GrillMasterOfflineException"/>
        public Task<OptimizedOrder> OptimizeGrillOrderAsync(GrillOrder grillOrder)
        {
            return OptimizeGrillOrderAsync(grillOrder, CancellationToken.None);
        }

        /// <summary>
        /// Optimize grill order.
        /// </summary>
        /// <param name="grillOrder"></param>
        /// <returns>Returns optimized grill order.</returns>
        /// <exception cref="GrillMasterInvalidJsonException"/>
        /// <exception cref="GrillMasterApiErrorException"/>
        /// <exception cref="GrillMasterOfflineException"/>
        public async Task<OptimizedOrder> OptimizeGrillOrderAsync(GrillOrder grillOrder, CancellationToken token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/grill/optimize");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Accept", "'application/problem+json'");
            request.Content = JsonContent.Create(grillOrder);

            var response = await _httpClient.SendAsync(request, token);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseStr = await response.Content.ReadAsStringAsync(token);
                    OptimizedOrder? optimizedOrder = JsonSerializer.Deserialize<OptimizedOrder>(responseStr, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                    if (optimizedOrder != null) return optimizedOrder;
                }
                catch(Exception ex) when (ex is JsonException || ex is NotSupportedException)
                {
                    throw new GrillMasterInvalidJsonException();
                }
            }
            else if(response.StatusCode == 0)
                throw new GrillMasterOfflineException();

            throw new GrillMasterApiErrorException();
        }

        public void Dispose() => _httpClient.Dispose();
    }
}
