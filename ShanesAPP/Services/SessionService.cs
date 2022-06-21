using ShanesAPP.Services.Interfaces;

namespace ShanesAPP.Services
{
    public class SessionService : ISessionService
    {
        private const string Token = "Token";
        private const string State = "State";

        public string GetToken(HttpContext httpContext)
        {
            return httpContext.Session.GetString(Token) ?? string.Empty;
        }

        public void SetToken(HttpContext httpContext, string token)
        {
            httpContext.Session.SetString(Token, token);
        }

        public string GetState(HttpContext httpContext)
        {
            return httpContext.Session.GetString(State) ?? string.Empty;
        }

        public void SetState(HttpContext httpContext, string token)
        {
            httpContext.Session.SetString(State, token);
        }

        public void ClearTokens(HttpContext httpContext)
        {
            httpContext.Session.Remove(Token);
            httpContext.Session.Remove(State);
        }
    }
}
