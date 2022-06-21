namespace ShanesAPP.Services.Interfaces
{
    public interface ISessionService
    {
        string GetToken(HttpContext httpContext);

        void SetToken(HttpContext httpContext, string token);

        string GetState(HttpContext httpContext);

        void SetState(HttpContext httpContext, string token);

        void ClearTokens(HttpContext httpContext);
    }
}
