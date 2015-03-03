using System.IO;
using System.Net;
using EasyGelf.Core;
using System;

namespace EasyGelf.Transport.Http {
    public sealed class HttpTransport : ITransport {
        private readonly HttpTransportConfiguration configuration;
        private readonly IGelfMessageSerializer messageSerializer;

        public HttpTransport(HttpTransportConfiguration configuration, IGelfMessageSerializer messageSerializer) {
            this.configuration = configuration;
            this.messageSerializer = messageSerializer;
        }

        public void Send(GelfMessage message) {

            var request = (HttpWebRequest)WebRequest.Create(configuration.ConnectionUri);
            request.Method = "POST";
            request.ReadWriteTimeout = request.Timeout = configuration.Timeout;
            request.ServicePoint.Expect100Continue = false; // See http://blogs.msdn.com/shitals/archive/2008/12/27/9254245.aspx

            // Basic Authentication Support
            // In case Graylog2 Server is behind a Firewall/Proxy/Load-Balancer, you can make use of Basic Auth (username+password)
            if (configuration.HasBasicAuthenticationSettings) {
                request.PreAuthenticate = true;
                request.Credentials = configuration.GetCredentialCache();
            }

            using (var requestStream = request.GetRequestStream())
            using (var messageStream = new MemoryStream(messageSerializer.Serialize(message)))
                messageStream.CopyTo(requestStream);

            using (var response = (HttpWebResponse)request.GetResponse()) {
                if (response.StatusCode == HttpStatusCode.Accepted)
                    return;

                throw new SendFailedException();
            }
        }

        public void Close() {
        }

    }
}