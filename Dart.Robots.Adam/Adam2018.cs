using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dartboard.Integration;

namespace Dart.Robots.Adam
{
    public class Adam2018 : AbstractRobot
    {
        private const string DeviceIpAddress = "localhost";

        public Adam2018() : base(new Uri($"http://{DeviceIpAddress}:5000"))
        {
            CameraAddresses.Add(new Uri($"http://{DeviceIpAddress}/?stream_0"));
            CameraAddresses.Add(new Uri($"http://{DeviceIpAddress}/?stream_1"));
        }
    }
}
