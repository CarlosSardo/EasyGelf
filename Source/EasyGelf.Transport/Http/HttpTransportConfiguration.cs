
using System;
using System.Net;
namespace EasyGelf.Transport.Http {
    public sealed class HttpTransportConfiguration {
        
        public string ConnectionUri { get; set; }
        public string BasicAuthenticationUsername { get; set; }
        public string BasicAuthenticationPassword { get; set; }
        public int Timeout { get; set; }

        public bool HasBasicAuthenticationSettings {
            get {
                return !String.IsNullOrWhiteSpace(this.BasicAuthenticationUsername) && !String.IsNullOrWhiteSpace(this.BasicAuthenticationPassword);
            }
        }

        public CredentialCache GetCredentialCache() {

            CredentialCache credentialCache = new CredentialCache();

            credentialCache.Add(new Uri(this.ConnectionUri),
                "Basic",
                new NetworkCredential(this.BasicAuthenticationUsername, this.BasicAuthenticationPassword));

            return credentialCache;
        }
    }
}