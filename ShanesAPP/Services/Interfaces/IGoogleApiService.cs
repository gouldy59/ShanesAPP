using ShanesAPP.Model;

namespace ShanesAPP.Services.Interfaces
{
    public interface IGoogleApiService
    {
        Task<GoogleUserResultModel> GetUserInfo(string Token);
    }
}
