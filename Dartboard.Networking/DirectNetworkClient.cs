using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Dartboard.Integration;
using Dartboard.Networking.Message;
using NLog;

namespace Dartboard.Networking
{
    public class DirectNetworkClient<TIncoming, TOutgoing> : AbstractNetworkClient<TIncoming, TOutgoing> where TOutgoing: Expireable
    {
        private readonly AbstractRobot _robot;
        private readonly IMessageFormatter<TIncoming> _inboundFormatter;
        private readonly IMessageFormatter<TOutgoing> _outboundFormatter;
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly Thread _heartbeatThread;
        private readonly Thread _outgoingThread;

        private CancellationToken _token;
        private TcpClient _client;

        public ConcurrentQueue<TIncoming> Inbox;
        public ConcurrentQueue<TOutgoing> Outbox;


        public DirectNetworkClient(AbstractRobot robot, IMessageFormatter<TIncoming> inboundFormatter, IMessageFormatter<TOutgoing> outboundFormatter)
        {
            _robot = robot;
            _inboundFormatter = inboundFormatter;
            _outboundFormatter = outboundFormatter;

            Inbox = new ConcurrentQueue<TIncoming>();
            Outbox = new ConcurrentQueue<TOutgoing>();

            _heartbeatThread = new Thread(Incoming);
            _outgoingThread = new Thread(Outgoing);

            try
            {
                _client = new TcpClient();
                _client.SendTimeout = 1000;
                _client.ReceiveTimeout = 1000;
                if (!Connect())
                    Log.Error("Unable to connect to robot!");
            }
            catch (SocketException e)
            {
                Log.Fatal(e);
            }
        }

        public void Start(CancellationToken token)
        {
            _token = token;
            _heartbeatThread.Start();
            _outgoingThread.Start();
        }

        private bool Connect()
        {

            Log.Info("Attempting to connect to " + _robot.DeviceEndpoint);
            _client = new TcpClient(_robot.DeviceEndpoint.Host, _robot.DeviceEndpoint.Port);
            return _client.Connected;
        }

        private void Incoming()
        {
            Log.Info("Start Incoming Thread");
            // What happens if client is not connected
            while (!_token.IsCancellationRequested)
            {
                try
                {
                    if (_client == null)
                        return;

                    if (!_client.Connected)
                        Connect();

                    if (_client.Connected)
                    {
                        using (var reader = new StreamReader(_client.GetStream()))
                        {
                            while (!_token.IsCancellationRequested)
                            {
                                if (reader.Peek() > 0)
                                {
                                    var inb = _inboundFormatter.Format(reader.ReadLine());
                                    if (Received != null)
                                    {
                                        Received(inb);
                                    }
                                    else
                                    {
                                        Inbox.Enqueue(inb);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    Thread.Sleep(10000);
                }
            }
        }

        private void Outgoing()
        {
            Log.Info("Start Outgoing Thread");
            while (!_token.IsCancellationRequested)
            {
                try
                {
                    if (Outbox.TryDequeue(out var msg))
                    {
                        if (msg.Expiration < DateTime.Now)
                        {
                            Log.Debug("Ignoring Expired Message");
                            continue;
                        }

                        if (_client == null)
                            return;

                        if (!_client.Connected)
                            Connect();

                        if (_client.Connected)
                        {
                            var body = _outboundFormatter.Format(msg);
                            _client.GetStream().Write(body, 0, body.Length);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    Thread.Sleep(1000);
                }
            }
        }

        public override void Send(TOutgoing msg)
        {
            Outbox.Enqueue(msg);
        }
    }
}