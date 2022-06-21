namespace ShanesAPP.Services.Interfaces
{
    public interface IGoogleApiClient
    {
        Task<HttpResponseMessage> GetUserInfo(string Token);
    }
}
