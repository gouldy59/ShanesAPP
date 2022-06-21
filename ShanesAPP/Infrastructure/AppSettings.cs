using ShanesAPP.Infrastructure.Interfaces;

namespace ShanesAPP.Infrastructure
{
    public class AppSettings : IAppSettings
    {
        public string ClientId { get; set; }

        public string ProjectId { get; set; }

        public string AuthUri { get; set; }

        public string TokenUri { get; set; }

        public string AuthProviderCert { get; set; }

        public string ClientSecret { get; set; }

        public string RedirecUri { get; set; }

        public string GoogleApi { get; set; }

        public AppSettings(IConfiguration configuration)
        {
            ClientId = configuration.GetValue<string>("client_id");
            ProjectId = configuration.GetValue<string>("project_id");
            AuthUri = configuration.GetValue<string>("auth_uri");
            TokenUri = configuration.GetValue<string>("token_uri");
            AuthProviderCert = configuration.GetValue<string>("auth_provider_x509_cert_url");
            ClientSecret = configuration.GetValue<string>("client_secret");
            RedirecUri = configuration.GetValue<string>("redirect_uris");
            GoogleApi = configuration.GetValue<string>("google_api");
        }
    }
}
