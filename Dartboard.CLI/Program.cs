using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dart.Robots.Adam;
using Dartboard.Integration;
using Dartboard.Networking;
using Dartboard.Networking.Json;
using Dartboard.Networking.Message;
using Dartboard.TheBox;
using Dartboard.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NLog;

namespace Dartboard.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.EnableLogging();
            ILogger log = LogManager.GetCurrentClassLogger();
            log.Info("Starting");

            Box box = null;
            try
            {
                box = Box.Locate();
            }
            catch (Exception e)
            {
                log.Error(e);
            }

            AbstractRobot robot = new Adam2018() { ColorWheel = true };

            var tokenSource = new CancellationTokenSource();

            // Must send types for simulator
            var ctrl = new RobotController(robot, TimeSpan.FromMilliseconds(50), box, sendTypes: false);

            ctrl.Start(tokenSource.Token);

            Console.ReadLine();

        }
    }
}
