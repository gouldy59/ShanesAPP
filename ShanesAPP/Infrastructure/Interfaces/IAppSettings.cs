namespace ShanesAPP.Infrastructure.Interfaces
{
    public interface IAppSettings
    {
        public string ClientId { get; set; }

        public string ProjectId { get; set; }

        public string AuthUri { get; set; }

        public string TokenUri { get; set; }

        public string AuthProviderCert { get; set; }

        public string ClientSecret { get; set; }

        public string RedirecUri { get; set; }
        public string GoogleApi { get; set; }
    }
}
