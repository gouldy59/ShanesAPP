using Newtonsoft.Json;
using ShanesAPP.Model;
using ShanesAPP.Services.Interfaces;

namespace ShanesAPP.Services
{
    public class GoogleApiService : IGoogleApiService
    {
        private readonly IGoogleApiClient _apiClient;

        public GoogleApiService(IGoogleApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GoogleUserResultModel> GetUserInfo(string Token)
        {
            var googleResultModel = new GoogleUserResultModel();
         
            var response = await _apiClient.GetUserInfo(Token);

            if (!response.IsSuccessStatusCode)
            {
                return ReturnErrorMessage(googleResultModel, $"Error Code: {(int)response.StatusCode} : Error Reason: {response.ReasonPhrase}");
            }

            var stringContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(stringContent))
            {
                return ReturnErrorMessage(googleResultModel, "No Content was returned");
            }

            googleResultModel = JsonConvert.DeserializeObject<GoogleUserResultModel>(stringContent);
            googleResultModel.IsValid = true;
            return googleResultModel;
        }

        private static GoogleUserResultModel ReturnErrorMessage(GoogleUserResultModel googleResultModel, string errorMessage)
        {
            googleResultModel.IsValid = false;
            googleResultModel.ErrorMessage = errorMessage; 
            return googleResultModel;
        }
    }
}
