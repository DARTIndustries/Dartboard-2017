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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NLog;

namespace Dartboard.CLI
{
    class Program
    {
        private const float ThrottleDelta = 0.05f;

        static void Main(string[] args)
        {
            LogManager.EnableLogging();
            ILogger log = LogManager.GetCurrentClassLogger();
            log.Info("Starting");

            AbstractRobot robot = new Adam2018();
            IMessageFormatter formatter = new JsonMessageFormatter();
            var anc = new DirectNetworkClient(robot, formatter);
            var tokenSource = new CancellationTokenSource();
            anc.Start(tokenSource.Token);

            float throttle = 1;

            while (true)
            {
                var pilot = GamePad.GetState(PlayerIndex.One);
                var captain = GamePad.GetState(PlayerIndex.Two);

                // Pilot Calculations

                var z = -pilot.Triggers.Left + pilot.Triggers.Right;
                var movementVector = Normalize(new Vector3(pilot.ThumbSticks.Left, z));
                movementVector *= throttle;

                /*
                 * Pitch    rX
                 * Roll     rY
                 * Yaw      rZ
                 */

                var rz = (pilot.IsButtonDown(Buttons.LeftShoulder) ? -1:0) + (pilot.IsButtonDown(Buttons.RightShoulder) ? 1 : 0);
                var headingVector = Normalize(new Vector3(pilot.ThumbSticks.Right.Y, pilot.ThumbSticks.Right.X, rz));
                headingVector *= throttle;

                if (pilot.IsButtonDown(Buttons.DPadDown))
                {
                    throttle -= ThrottleDelta;
                    if (throttle < 0)
                        throttle = 0;
                }

                if (pilot.IsButtonDown(Buttons.DPadUp))
                {
                    throttle += ThrottleDelta;
                    if (throttle > 1)
                        throttle = 1;
                }

                if (pilot.IsButtonDown(Buttons.Y))
                    throttle = 1;

                if (pilot.IsButtonDown(Buttons.X))
                    throttle = 0;

                // Captain Calculations

                var msg = new DoRequestMessage()
                {
                    Do = new IndirectDoElement()
                    {
                        MovementVector = movementVector,
                        Heading = headingVector,
                        Lights = Color.Beige
                    }
                };
                anc.Outbox.Enqueue(msg);
                Thread.Sleep(50);
            }

            Console.ReadLine();

        }

        private static Vector3 Normalize(Vector3 vec)
        {
            var max = (new[] {Math.Abs(vec.X), Math.Abs(vec.Y), Math.Abs(vec.Z)}).Max();
            vec.Normalize();
            return vec * max;
        }
    }
}
