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
using Dartboard.Utils;
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
            //float xmin = 0, ymin = 0, xmax = 0, ymax = 0;

            //List<Point> points = new List<Point>();

            //for (int i = 0; i < 5000; i++)
            //{
            //    var s = GamePad.GetState(PlayerIndex.One);

            //    var x = s.ThumbSticks.Left.X;
            //    var y = s.ThumbSticks.Left.Y;

            //    points.Add(new Point(x.ToSByte(), y.ToSByte()));
            //    Thread.Sleep(1);
            //}

            //foreach (var point in points.Distinct())
            //{
            //    Console.WriteLine($"({point.X}, {point.Y})");

            //}

            //Console.ReadLine();


            LogManager.EnableLogging();
            ILogger log = LogManager.GetCurrentClassLogger();
            log.Info("Starting");

            AbstractRobot robot = new Adam2018();
            var heartbeatFormatter = new JsonMessageFormatter<Heartbeat>();
            var msgFormatter = new JsonMessageFormatter<DoRequestMessage>();
            var anc = new DirectNetworkClient<Heartbeat, DoRequestMessage>(robot, heartbeatFormatter, msgFormatter);
            var tokenSource = new CancellationTokenSource();
            anc.Start(tokenSource.Token);

            float throttle = 1;

            while (true)
            {
                var pilot = GamePad.GetState(PlayerIndex.One);
                var captain = GamePad.GetState(PlayerIndex.Two);

                // Pilot Calculations

                var z = (pilot.IsButtonDown(Buttons.LeftShoulder) ? -1:0) + (pilot.IsButtonDown(Buttons.RightShoulder) ? 1 : 0);
                var movementVector = Normalize(new Vector3(pilot.ThumbSticks.Left, z));
                movementVector *= throttle;

                /*
                 * Pitch    rX
                 * Roll     rY
                 * Yaw      rZ
                 */

                var rz = -pilot.Triggers.Left + pilot.Triggers.Right;
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

                bool zeroOverride =  pilot.IsButtonDown(Buttons.B);

                // Captain Calculations

                var cameraVector = Normalize(captain.ThumbSticks.Left);
                //var cameraMovement = new ServoElement()
                //{
                //    Velocity = new byte[] { cameraVector.X.ToByte(0, 180), cameraVector.Y.ToByte(0, 180)}
                //}


                var msg = new DoRequestMessage(TimeSpan.FromSeconds(2))
                {
                    Do = new IndirectDoElement()
                    {
                        MotorVector = new MotorVector()
                        {
                            Velocity = movementVector,
                            AngularVelocity = headingVector
                        },
                        //Lights = robot.GetColor()
                    }
                };


                if (zeroOverride)
                {
                    var oldDo = msg.Do;
                    msg.Do = new DirectDoElement()
                    {
                        Motors = new sbyte[robot.Motors.Count],
                        Camera = oldDo.Camera,
                        Claw = oldDo.Claw,
                        //Lights = oldDo.Lights
                    };
                }

                anc.Outbox.Enqueue(msg);
                Thread.Sleep(50);
            }

            Console.ReadLine();

        }

        private static Vector3 Normalize(Vector3 vec)
        {
            return vec;
            var max = (new[] {Math.Abs(vec.X), Math.Abs(vec.Y), Math.Abs(vec.Z)}).Max();
            vec.Normalize();
            return vec * max;
        }

        private static Vector2 Normalize(Vector2 vec)
        {
            return vec;
            var max = (new[] { Math.Abs(vec.X), Math.Abs(vec.Y)}).Max();
            vec.Normalize();
            return vec * max;
        }
    }
}
