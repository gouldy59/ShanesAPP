namespace ShanesAPP.Services.Interfaces
{
    public interface IAuthClient
    {
        public Task<string> GetTokenAsync(string code);
    }
}
