using ShanesAPP.Model;

namespace ShanesAPP.Services.Interfaces
{
    public interface IAuthorizationService
    {
        string GetUri(HttpContext httpContext);

        Task<TokenResultModel> GetToken(HttpContext httpContext, string code);

        bool IsValidateState(HttpContext httpContext, string state);

        bool IsTokenValid(string token);
    }
}
