using RestSharp;
using GrillMaster.Client.AppConfiguration;
using GrillMaster.Client.Exceptions;
using RestSharp.Authenticators;
using GrillMaster.Client.DTO;

namespace GrillMaster.Client
{
    internal class GrillMasterClient
    {
        private readonly AppConfig _appConfig;
        private readonly RestClient _restClient;

        public GrillMasterClient(AppConfig appConfig)
        {
            _appConfig = appConfig;
            _restClient = new RestClient(_appConfig.Server.Uri);
        }
        
        public string DoGrillOrder(string jsonOrder)
        {
            if (_restClient.Authenticator is null) Authenticate();

            RestRequest request = new ("/api/grill/optimize", Method.Post);
            request.AddBody(jsonOrder, "application/json");
            RestResponse response = _restClient.Execute(request);
            if (response.IsSuccessful && response.Content != null)
            {
                return response.Content;
            }
            throw new GrillMasterApiErrorException();
        }

        private void Authenticate()
        {
            RestRequest request = new ("/api/user/login", Method.Post);
            request.AddJsonBody(new AuthUserRequest(_appConfig.Auth.UserName, _appConfig.Auth.Password));
            RestResponse response = _restClient.Execute(request);
            if (response.StatusCode == 0) throw new GrillMasterOfflineException();
            if(response.IsSuccessful && response.Content != null)
            {
                // Token string in response.Content is received encapsulated within quotes "__TOKEN__"
                // Workaround - remove quotes.
                _restClient.Authenticator = new JwtAuthenticator(response.Content.Replace("\"", ""));
                return;
            }
            throw new GrillMasterNotAuthenticatedException();
        }
    }
}
