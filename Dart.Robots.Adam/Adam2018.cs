using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dartboard.Integration;
using Microsoft.Xna.Framework;
using SysColor = System.Drawing.Color;

namespace Dart.Robots.Adam
{
    public class Adam2018 : AbstractRobot
    {
        private const string DeviceIpAddress = "129.25.218.183";
        //private const string DeviceIpAddress = "localhost";
        //private const string DeviceIpAddress = "10.0.0.2";
        private DateTime startupTime;

        public bool ColorWheel { get; set; }

        public Adam2018() : base(new Uri($"http://{DeviceIpAddress}:5000"))
        {
            CameraAddresses.Add(new Uri($"http://{DeviceIpAddress}/?stream_0"));
            CameraAddresses.Add(new Uri($"http://{DeviceIpAddress}/?stream_1"));

            Motors.Add(new Motor());
            Motors.Add(new Motor());

            Motors.Add(new Motor());
            Motors.Add(new Motor());

            Motors.Add(new Motor());
            Motors.Add(new Motor());

            startupTime = DateTime.Now;
        }

        public override Color GetColor()
        {
            if (!ColorWheel)
                return Color.CornflowerBlue;

            var diff = DateTime.Now - startupTime;
            var angle = (Math.Sin(diff.TotalSeconds/2) + 1) * 180;
            var c = Dartboard.Utils.Utility.ColorFromHSV(angle, 1, 1);
            return new Color(c.R, c.G, c.B, c.A);
        }
    }
}
