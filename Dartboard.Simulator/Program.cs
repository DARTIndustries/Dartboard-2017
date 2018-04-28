using Dart.Robots.Adam;
using Dartboard.Integration;
using Dartboard.Networking;
using Dartboard.Networking.Json;
using Dartboard.Networking.Message;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Dartboard.Simulator
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            while (true)
            {
                try
                {
                    AbstractRobot robot = new Adam2018();
                    var heartbeatFormatter = new JsonMessageFormatter<Heartbeat>();
                    var msgFormatter = new JsonMessageFormatter<DoRequestMessage>();
                    var anc = new DirectNetworkClient<DoRequestMessage, Heartbeat>(robot, msgFormatter, heartbeatFormatter);
                    var tokenSource = new CancellationTokenSource();
                    anc.Start(tokenSource.Token);

                    anc.Received += (msg) =>
                    {
                        Console.WriteLine(System.Text.Encoding.UTF8.GetString(msgFormatter.Format(msg)));
                    };
                    
                }
                catch (Exception) { }
            }

            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
