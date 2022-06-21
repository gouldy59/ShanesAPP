using IdentityModel.Client;
using ShanesAPP.Infrastructure.Interfaces;
using ShanesAPP.Services.Interfaces;

namespace ShanesAPP.Services
{
    public class GoogleApiClient : IGoogleApiClient
    {
        private readonly HttpClient _httpClient;

        public GoogleApiClient(HttpClient httpClient, IAppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.GoogleApi);
        }

        public async Task<HttpResponseMessage> GetUserInfo(string token)
        {
            _httpClient.SetBearerToken(token);
            return await _httpClient.GetAsync("userinfo");
        }
    }
}
