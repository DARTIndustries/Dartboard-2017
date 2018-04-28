using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Mjpeg;

namespace Sandbox
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new MJpegClient(new Uri("http://10.0.0.77:8080/?action=stream"));

            int counter = 0;

            client.FrameReceived += image => image.Save(counter++ + ".jpg");

            var task = client.Get();


            Console.ReadLine();
        }
    }
}
