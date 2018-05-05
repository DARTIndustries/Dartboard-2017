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
                    var msgFormatter = new JsonMessageFormatter<DoRequestMessage>();
                    msgFormatter.IncludeTypes = true;

                    TcpListener l = new TcpListener(IPAddress.Any, 5000);
                    l.Start();
                    while (true)
                    {
                        //Console.Clear();
                        Console.WriteLine("Awaiting Client...");
                        var client = l.AcceptTcpClient();
                        using (var reader = new StreamReader(client.GetStream()))
                        {
                            while (client.Connected)
                            {
                                var msg = reader.ReadLine();
                                Console.WriteLine(msg);
                                try
                                {
                                    var command = msgFormatter.Format(msg);
                                    if (command.Do is DirectDoElement dido)
                                    {
                                        Console.WriteLine("Motor Values: " + string.Join(", ", dido.Motors));
                                    }
                                    else if (command.Do is IndirectDoElement iddo)
                                    {
                                        Console.WriteLine(iddo.MotorVector);
                                    }

                                    if (command.Do.Buzzer != null)
                                    {
                                        Console.WriteLine("Buzzer: " + (command.Do.Buzzer.State ? "On":"Off"));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error while deserializing.");
                                }
                            }
                        }
                    }
                }
                catch (Exception) { }
            }

            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
