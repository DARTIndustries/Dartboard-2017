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
    public class DirectNetworkClient : AbstractNetworkClient
    {
        private readonly AbstractRobot _robot;
        private readonly IMessageFormatter _formatter;
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly Thread _heartbeatThread;
        private readonly Thread _outgoingThread;

        private CancellationToken _token;
        private TcpClient _client;

        public ConcurrentQueue<Heartbeat> Inbox;
        public ConcurrentQueue<DoRequestMessage> Outbox;


        public DirectNetworkClient(AbstractRobot robot, IMessageFormatter formatter)
        {
            _robot = robot;
            _formatter = formatter;

            Inbox = new ConcurrentQueue<Heartbeat>();
            Outbox = new ConcurrentQueue<DoRequestMessage>();

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
                                    var heartbeat = _formatter.Format(reader.ReadLine());
                                    Inbox.Enqueue(heartbeat);
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
                        if (_client == null)
                            return;

                        if (!_client.Connected)
                            Connect();

                        if (_client.Connected)
                        {
                            var body = _formatter.Format(msg);
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

        public override void Send(DoRequestMessage msg)
        {
            Outbox.Enqueue(msg);
        }
    }
}