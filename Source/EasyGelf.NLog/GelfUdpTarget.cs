﻿using System.Linq;
using System.Net;
using EasyGelf.Core;
using EasyGelf.Core.Encoders;
using EasyGelf.Transport;
using EasyGelf.Transport.Udp;
using NLog.Targets;

namespace EasyGelf.NLog
{
    [Target("GelfUdp")]
    public sealed class GelfUdpTarget : GelfTargetBase
    {
        public GelfUdpTarget()
        {
            RemoteAddress = IPAddress.Loopback.ToString();
            RemotePort = 12201;
            MessageSize = 8096;
        }

        public string RemoteAddress { get; set; }

        public int RemotePort { get; set; }

        public int MessageSize { get; set; }

        protected override ITransport InitializeTransport(IEasyGelfLogger logger)
        {
            var encoder = new CompositeEncoder(new GZipEncoder(), new ChunkingEncoder(new MessageBasedIdGenerator(), MessageSize.UdpMessageSize()));
            var removeIpAddress = Dns.GetHostAddresses(RemoteAddress)
                .Shuffle()
                .FirstOrDefault() ?? IPAddress.Loopback;
            var configuration = new UdpTransportConfiguration
                {
                    Host = new IPEndPoint(removeIpAddress, RemotePort)
                };
            return new UdpTransport(configuration, encoder, new GelfMessageSerializer());
        }
    }
}