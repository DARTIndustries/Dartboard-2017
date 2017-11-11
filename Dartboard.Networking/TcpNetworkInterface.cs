using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DART.Dartboard.Models;
using DART.Dartboard.Utils;

namespace DART.Dartboard.Networking
{
    public class TcpNetworkInterface : LoggableObject, INetworkInterface
    {
        private readonly IMessageFormatter _formatter;
        private readonly Uri _endpoint;
        private readonly TcpClient _client;

        public TcpNetworkInterface(IMessageFormatter formatter, Uri endpoint)
        {
            _formatter = formatter;
            _endpoint = endpoint;

            if (endpoint.Scheme != "tcp")
                throw new ProtocolViolationException("URI Must Be Of Type tcp://");

            if (endpoint.Port == -1)
                throw new ArgumentException("URI Must Include Port");

            try
            {
                _client = new TcpClient(endpoint.Host, endpoint.Port);
            }
            catch (SocketException e)
            {
                Log.Fatal(e);
            }
        }

        public void Send(Do message)
        {
            if (_client != null && _client.Connected)
            {
                var body = _formatter.Format(message);
                _client.GetStream().Write(body, 0, body.Length);
            }
        }
    }
}
