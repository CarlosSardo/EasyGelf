using System.Net;
using EasyGelf.Core;
using EasyGelf.Core.Encoders;
using System.Linq;
using EasyGelf.Transport;
using EasyGelf.Transport.Http;

namespace EasyGelf.Log4Net
{
    public sealed class GelfHttpAppender : GelfAppenderBase
    {
        public GelfHttpAppender()
        {
            ConnectionUri = string.Empty;
            BasicAuthenticationUsername = string.Empty;
            BasicAuthenticationPassword = string.Empty;
            Timeout = 60000;
        }

        public string ConnectionUri { get; set; }

        public int Timeout { get; set; }

        public string BasicAuthenticationUsername { get; set; }

        public string BasicAuthenticationPassword { get; set; }

        protected override ITransport InitializeTransport(IEasyGelfLogger logger)
        {
            var configuration = new HttpTransportConfiguration
                {
                    ConnectionUri = ConnectionUri,
                    BasicAuthenticationUsername = BasicAuthenticationUsername,
                    BasicAuthenticationPassword = BasicAuthenticationPassword,
                    Timeout = Timeout,
                };
            
            return new HttpTransport(configuration, new GelfMessageSerializer());
        }
    }
}