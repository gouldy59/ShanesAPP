using IdentityModel.Client;
using ShanesAPP.Infrastructure;
using ShanesAPP.Infrastructure.Interfaces;
using ShanesAPP.Model;
using ShanesAPP.Services.Interfaces;

namespace ShanesAPP.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAppSettings _appSettings;
        private readonly IAuthClient _authClient;
        private readonly ISessionService _sessionService;

        public AuthorizationService(IAppSettings appSettings, IAuthClient authClient, ISessionService sessionService)
        {
            _appSettings = appSettings;
            _authClient = authClient;
            _sessionService = sessionService;
        }

        public async Task<TokenResultModel> GetToken(HttpContext httpContext, string code)
        {
            var tokenResultModel = new TokenResultModel();
         
            tokenResultModel.AccessToken = await _authClient.GetTokenAsync(code);

            if (!IsTokenValid(tokenResultModel.AccessToken))
            {
                tokenResultModel.IsValid = false;
                return tokenResultModel;
            }
            
            tokenResultModel.IsValid = true;
            _sessionService.SetToken(httpContext, tokenResultModel.AccessToken);

            return tokenResultModel;
        }

        public bool IsTokenValid(string token)
        {
            // Add any additional checks
            return !string.IsNullOrEmpty(token);
        }

        public bool IsValidateState(HttpContext httpContext, string state)
        {
            return _sessionService.GetState(httpContext) == state;
        }

        public string GetUri(HttpContext httpContext)
        {
            var state = Guid.NewGuid().ToString();
            _sessionService.SetState(httpContext, state);

            return $"{_appSettings.AuthUri}?client_id={_appSettings.ClientId}&response_type=code" +
                $"&scope={Constants.Scope}&redirect_uri={_appSettings.RedirecUri}&state={state}";
        }
    }
}
