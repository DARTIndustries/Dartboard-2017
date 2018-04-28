using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

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

                    TcpListener listener = new TcpListener(IPAddress.Any, 5000);
                    listener.Start();
                    while (true)
                    {
                        Console.Write("Awaiting Client: ");
                        var client = listener.AcceptTcpClient();
                        Console.WriteLine("Connected.");
                        client.GetStream().CopyTo(Console.OpenStandardOutput());
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
