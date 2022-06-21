using IdentityModel.Client;
using ShanesAPP.Infrastructure.Interfaces;
using ShanesAPP.Services.Interfaces;

namespace ShanesAPP.Services
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAppSettings _appSettings;

        public AuthClient(HttpClient httpClient, IAppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.TokenUri);
            _appSettings = appSettings;
        }

        public async Task<string> GetTokenAsync(string code)
        {
            var response = await _httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            {
                ClientId = _appSettings.ClientId,
                ClientSecret = _appSettings.ClientSecret,
                Code = code,
                RedirectUri = _appSettings.RedirecUri
            });

            if (response.IsError)
                throw new Exception(response.Error); // This is bad and should be refactored.

            return response.AccessToken;
        }
    }
}
